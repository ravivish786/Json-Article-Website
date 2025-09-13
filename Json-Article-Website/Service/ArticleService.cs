using System.Text;
using System.Text.Json;
using System.Web;
using HtmlAgilityPack;
using Json_Article_Website.Extention;
using Json_Article_Website.Helper;
using Json_Article_Website.Interface;
using Json_Article_Website.Models;
using Json_Article_Website.Service.Views;

namespace Json_Article_Website.Service
{
    public class ArticleService(IWebHostEnvironment webHost, ILogger<ArticleService> _logger, IViewCounterService viewCounter) 
        : IArticleService
    {
        private readonly FileService fileService = new(webHost);
        private readonly ImageGeneratorService imageGenerator = new(webHost);

        public async Task<ArticleDetailsModel?> GetArticleDetailsAsync(int id, CancellationToken cancellationToken )
        {
 
            // Simulate fetching articles from a data source
            var fullPath = fileService.GetArticlePath(id);
            var bytes = await fileService.ReadFileAsync(fullPath);

            if (bytes == null)
            {
                return default;
            }
            // Increment view count for the article
            viewCounter.IncrementView(id);

            // Deserialize the byte array to ArticleDetailsModel
            var articles = JsonSerializer.Deserialize<ArticleDetailsModel>(bytes);
            return articles;
        }

        public async Task<ArticlesList> GetArticlesAsync(int? _page, bool IsAdmin = false, CancellationToken cancellationToken = default)
        {
            if (_page == null)
            {
                var response = await GetFilesMetaDataAsync();
                _page = response.LastIndexFile;
            }

            int page = _page.Value;

            // Simulate fetching articles from a data source
            var fullPath = fileService.GetIndexPath(page);
            var bytes = await fileService.ReadFileAsync(fullPath);

            if (bytes == null)
            {
                return default;
            }
            var articles = JsonSerializer.Deserialize<IEnumerable<ArticleIndexModel>>(bytes);

            var data = new ArticlesList
            {
                Page = page - 1,
                Articles = articles?.Where(x=> x.IsPublished || IsAdmin)?.OrderByDescending(x => x.PublishedDate),
                HasNextPage = true
            };

            return data;
        }

        public async Task<ArticleDetailsModel> PostArticleAsync(ArticleDetailsModel article, CancellationToken cancellationToken)
        {
            var metadata = await GetFilesMetaDataAsync();
            metadata.LastArticleId += 1;
            article.Id = metadata.LastArticleId;
            article.IndexFileName = metadata.LastIndexFile;
             
            var filePath = fileService.GetArticlePath(article.Id);
            if (File.Exists(filePath))
            {
                throw new InvalidOperationException($"Article with ID {article.Id} already exists.");
            }

            article.ImageUrl = string.IsNullOrWhiteSpace(article.ImageUrl) 
                ? imageGenerator.GenerateDefaultImage(article.Title, "Admin", "Linkker")
                : article.ImageUrl;

            article.ReadingTime = article.Content?.GetEstimatedReadTime() ?? 0;

            var bytes = JsonSerializer.SerializeToUtf8Bytes(article) ?? throw new ArgumentNullException(nameof(article), "Article cannot be null");
            if (bytes.Length == 0)
            {
                throw new ArgumentException("Article cannot be empty", nameof(article));
            }
            await fileService.SaveFileAsync(filePath, bytes);

            // update index of article 
            await UpdateFilesIndex(new ArticleIndexModel
            {
                Id = article.Id,
                Title = article.Title,
                Slug = article.Slug,
                PublishedDate = article.PublishedDate,
                Views = article.Views,
                Likes = article.Likes,
                ReadingTime = article.ReadingTime,
            }, metadata);
            return article;
        }

        public async Task<ArticleDetailsModel> PutArticleAsync(int id, ArticleDetailsModel article, CancellationToken cancellationToken)
        {
            var fullPath = fileService.GetArticlePath(id);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Article with ID {id} does not exist.");
            }


            // If image is not provided, keep the existing one
            var existingArticle = await this.GetArticleDetailsAsync(id, cancellationToken);
            if (existingArticle != null && !string.IsNullOrWhiteSpace(article.ImageUrl) && existingArticle.Title.Equals(article.Title, StringComparison.OrdinalIgnoreCase))
            {
                article.ImageUrl = existingArticle.ImageUrl;
            }
            else
            {
                article.ImageUrl = imageGenerator.GenerateDefaultImage(article.Title, "Admin", "Linkker");
            }
            //article.Likes += existingArticle?.Likes ?? 0;
            //article.Views += existingArticle?.Views ?? 0;
           
            article.ReadingTime = article.Content?.GetEstimatedReadTime() ?? 0;

            var bytes = JsonSerializer.SerializeToUtf8Bytes(article) ?? throw new ArgumentNullException(nameof(article), "Article cannot be null");
            if (bytes.Length == 0)
            {
                throw new ArgumentException("Article cannot be empty", nameof(article));
            }
            await fileService.SaveFileAsync(fullPath, bytes);

            // update index of article
            await this.UpdateIndexFileContentAsync(article.IndexFileName, new ArticleIndexModel
            {
                Id = article.Id,
                Title = article.Title,
                Slug = article.Slug,
                PublishedDate = article.PublishedDate,
                Views = article.Views,
                Likes = article.Likes,
                ReadingTime = article.ReadingTime,
            }, Delete: false);

            return article;
        }

        public async Task<bool> DeleteArticleAsync(int id, CancellationToken cancellationToken)
        {
            var fullPath = fileService.GetArticlePath(id);

            var article = await this.GetArticleDetailsAsync(id, cancellationToken);

            // update index file but if file if open or read then 

            if (article != null) {
                await this.UpdateIndexFileContentAsync(article.IndexFileName, new ArticleIndexModel
                {
                    Id = article.Id,
                }, Delete: true);
            }


            bool exists = await fileService.DeleteFileAsync(fullPath);
            return exists;
        }


        public async Task<ScrapeArticleModel> ScrapeArticleAsync(string url, CancellationToken cancellationToken)
        {
            var result = new ScrapeArticleModel();
            var decodeUrl = HttpUtility.UrlDecode(url);
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                
                var html = await httpClient.GetStringAsync(decodeUrl, cancellationToken);

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                // Try Open Graph tags first
                result.Title = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:title']")?.GetAttributeValue("content", null);
                result.Description = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:description']")?.GetAttributeValue("content", null);
                result.Image = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:image']")?.GetAttributeValue("content", null);

                // Fallbacks
                if (string.IsNullOrEmpty(result.Title))
                    result.Title = htmlDoc.DocumentNode.SelectSingleNode("//title")?.InnerText;

                if (string.IsNullOrEmpty(result.Description))
                    result.Description = htmlDoc.DocumentNode.SelectSingleNode("//meta[@name='description']")?.GetAttributeValue("content", null);
            }
            catch (Exception ex)
            {
                result = new ScrapeArticleModel();
                _logger.LogError(ex, "Error scraping article from URL: {decodeUrl}", decodeUrl);
            }

            return result;
        }


        #region Increment /Decrement Views

        public async Task IncrementViewAsync(int articleId, int count, CancellationToken cancellationToken)
        {
            //var article = await _dbContext.Articles.FindAsync(new object[] { articleId }, cancellationToken);
            //if (article != null)
            //{
            //    article.Views += count;
            //    await _dbContext.SaveChangesAsync(cancellationToken);
            //}
        }


        #endregion


        #region Helper Methods

        private string MetaDataPath => Path.Combine(fileService._appDataPath, "metadata.json");

        private async Task<ArticleFileMetadata> GetFilesMetaDataAsync()
        {
            var reponse = await fileService.ReadFileAsync(MetaDataPath);

            var defaultResponse = new ArticleFileMetadata { };

            return reponse == null
                ? defaultResponse
                : JsonSerializer.Deserialize<ArticleFileMetadata>(reponse) ?? defaultResponse;
        }
         
        private async Task<ArticleFileMetadata> UpdateFilesMetadata(ArticleFileMetadata fileMetadata)
        {
            await fileService.SaveFileAsync(MetaDataPath, JsonSerializer.SerializeToUtf8Bytes(fileMetadata));
            return fileMetadata;
        }

        private async Task UpdateIndexFileContentAsync(int indexPageNumber, ArticleIndexModel model, bool? Delete = null)
        {
            var indexFile = fileService.GetIndexPath(indexPageNumber);
            var bytes = await fileService.ReadFileAsync(indexFile) ?? [];
            var articles = bytes == null || bytes.Length == 0
                ? [] 
                : JsonSerializer.Deserialize<IList<ArticleIndexModel>>(bytes) ?? [];

            // fetch index and replce with model
            var index = articles.IndexOf(articles.FirstOrDefault(x => x.Id == model.Id));
            if (index >= 0)
            {
                if (Delete == true)
                {
                    // remove from list 
                    articles.RemoveAt(index);
                }
                else
                {
                    articles[index] = model;
                }
                await UpdateIndexFileAsync(indexFile, articles);
            }
            
        }

        private async Task UpdateIndexFileAsync(string indexFilePath, IList<ArticleIndexModel> articles )
        {
            if (articles.Count == 0)
            {
                await fileService.SaveFileAsync(indexFilePath, []);
                return;
            }
            var bytes = JsonSerializer.SerializeToUtf8Bytes(articles);
            await fileService.SaveFileAsync(indexFilePath, bytes);
        }

        private async Task<ArticleFileMetadata> UpdateFilesIndex(ArticleIndexModel article, ArticleFileMetadata metadata)
        {
            var _metadata = metadata ?? await GetFilesMetaDataAsync() ;

            _metadata.LastArticleId = article.Id;

            string indexFilePath = fileService.GetIndexPath(_metadata.LastIndexFile );
            var bytes = await fileService.ReadFileAsync(indexFilePath) ;
            var articles = bytes == null ? [] : JsonSerializer.Deserialize<IList<ArticleIndexModel>>(bytes) ?? [];
             
            if (articles.Count >= ArticleMetaConst.MaxRowsPerIndexFile)
            {
                _metadata.LastIndexFile +=1;
                articles = [article];
                indexFilePath = fileService.GetIndexPath(_metadata.LastIndexFile);
            }
            else
            {
                articles.Add(article);
            }
            await this.UpdateIndexFileAsync(indexFilePath, articles);
            await this.UpdateFilesMetadata(_metadata);
            return _metadata;
        }

        #endregion

    }
}

using System.Text.Json;
using Json_Article_Website.Helper;
using Json_Article_Website.Interface;
using Json_Article_Website.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;

namespace Json_Article_Website.Service
{
    public class ArticleService(IWebHostEnvironment webHost) : IArticleService
    {
        private readonly FileService fileService = new FileService(webHost);


        public async Task<ArticleDetailsModel?> GetArticleDetailsAsync(int id )
        {
 
            // Simulate fetching articles from a data source
            var fullPath = fileService.GetArticlePath(id);
            var bytes = await fileService.ReadFileAsync(fullPath);

            if (bytes == null)
            {
                return default;
            }
            var articles = JsonSerializer.Deserialize<ArticleDetailsModel>(bytes);
            return articles;
        }

        public async Task<ArticlesList> GetArticlesAsync(int? _page, bool IsAdmin = false)
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

        public async Task<ArticleDetailsModel> PostArticleAsync(ArticleDetailsModel article)
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
            var bytes = JsonSerializer.SerializeToUtf8Bytes(article) ?? throw new ArgumentNullException(nameof(article), "Article cannot be null");
            if (bytes.Length == 0)
            {
                throw new ArgumentException("Article cannot be empty", nameof(article));
            }
            await fileService.SaveFileAsync(filePath, bytes);

            // update index of article 
            metadata = await UpdateFilesIndex(new ArticleIndexModel
            {
                Id = article.Id,
                Title = article.Title,
                Slug = article.Slug,
                PublishedDate = article.PublishedDate
            }, metadata);
            return article;
        }

        public async Task<ArticleDetailsModel> PutArticleAsync(int id, ArticleDetailsModel article)
        {
            var fullPath = fileService.GetArticlePath(id);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Article with ID {id} does not exist.");
            }

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
                PublishedDate = article.PublishedDate
            }, Delete: false);

            return article;
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            var fullPath = fileService.GetArticlePath(id);

            var article = await this.GetArticleDetailsAsync(id);

            // update index file 
            if (article != null) {
                await this.UpdateIndexFileContentAsync(article.IndexFileName, new ArticleIndexModel
                {
                    Id = article.Id,
                }, Delete: true);
            }


            bool exists = await fileService.DeleteFileAsync(fullPath);
            return exists;
        }



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
            var bytes = await fileService.ReadFileAsync(indexFile) ?? Array.Empty<byte>();
            var articles = bytes == null 
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
                await fileService.SaveFileAsync(indexFilePath, Array.Empty<byte>());
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

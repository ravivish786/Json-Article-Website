using System.Text.Json;
using Json_Article_Website.Helper;
using Json_Article_Website.Interface;
using Json_Article_Website.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public async Task<IEnumerable<ArticleIndexModel>?> GetArticlesAsync(int page = 1)
        {
             
            if (page < 1)
            {
                page = 1;
            }
            // Simulate fetching articles from a data source
            var fullPath = fileService.GetIndexPath(page);
            var bytes = await fileService.ReadFileAsync(fullPath);

            if (bytes == null)
            {
                return default;
            }
            var articles = JsonSerializer.Deserialize<IEnumerable<ArticleIndexModel>>(bytes);
            return articles;
        }

        public async Task<ArticleDetailsModel> PostArticleAsync(ArticleDetailsModel article)
        {
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
            return  article;
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            var fullPath = fileService.GetArticlePath(id);
            bool exists = await fileService.DeleteFileAsync(fullPath);
            return exists;
        }



        #region Helper Methods

        private string MetaDataPath => Path.Combine(fileService._appDataPath, "metadata.json");

        private async Task<ArticleFileMetadata> GetFilesMetaData()
        {
            var reponse = await fileService.ReadFileAsync(MetaDataPath);
            var defaultResponse = new ArticleFileMetadata { };
            return reponse == null
                ? defaultResponse
                : JsonSerializer.Deserialize<ArticleFileMetadata>(reponse) ?? defaultResponse;
        }
         
        private async Task<ArticleFileMetadata> UpdateFilesIndex(ArticleIndexModel article, ArticleFileMetadata metadata)
        {
            var _metadata = metadata ?? await GetFilesMetaData() ;

            _metadata.LastArticleId = article.Id;

            string filePath = fileService.GetIndexPath(_metadata.LastIndexFile );
            var bytes = await fileService.ReadFileAsync(filePath);
            var articles = JsonSerializer.Deserialize<IList<ArticleIndexModel>>(bytes) ?? [];
             
            if (articles.Count >= ArticleMetaConst.MaxRowsPerIndexFile)
            {
                _metadata.LastIndexFile +=1;
                articles = [article];
                filePath = fileService.GetIndexPath(_metadata.LastIndexFile);
            }

            await fileService.SaveFileAsync(filePath, JsonSerializer.SerializeToUtf8Bytes(articles));

            return _metadata;
        }

        #endregion

    }
}

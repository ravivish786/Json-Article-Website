using Json_Article_Website.Models;

namespace Json_Article_Website.Interface
{
    public interface IArticleService
    {
         
        /// <summary>
        /// Fetches the details of an article by its ID and optional slug.
        /// </summary>
        /// <param name="id">The ID of the article.</param>
        /// <param name="slug">An optional slug for the article.</param>
        /// <returns>A task that represents the asynchronous operation, containing the article details.</returns>
        Task<ArticleDetailsModel?> GetArticleDetailsAsync(int id );
        /// <summary>
        /// Fetches a paginated list of articles.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of articles.</returns>
        Task<ArticlesList> GetArticlesAsync(int? _page);


        Task<ArticleDetailsModel> PostArticleAsync(ArticleDetailsModel article);
        Task<ArticleDetailsModel> PutArticleAsync(int id, ArticleDetailsModel article);
        Task<bool> DeleteArticleAsync(int id);


    }
}

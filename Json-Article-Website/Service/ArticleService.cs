using Json_Article_Website.Interface;
using Json_Article_Website.Models;

namespace Json_Article_Website.Service
{
    public class ArticleService : IArticleService
    {
        public Task<ArticleDetailsModel> GetArticleDetailsAsync(int id )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArticleIndexModel>> GetArticlesAsync(int page = 1)
        {
            throw new NotImplementedException();
        }
    }
}

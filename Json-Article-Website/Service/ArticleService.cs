using Json_Article_Website.Interface;
using Json_Article_Website.Models;

namespace Json_Article_Website.Service
{
    public class ArticleService : IArticleService
    {
        
        public Task<ArticleDetailsModel> GetArticleDetailsAsync(int id )
        {
            //throw new NotImplementedException();
            return Task.FromResult( new ArticleDetailsModel()
            {
                Id = id,
                Title = "Sample Article",
                Slug = "sample-article",
                ImageUrl = "/images/sample.jpg",
                Content = "This is a sample article content.",
                CreatedDate = DateTime.UtcNow
            });
        }

        public Task<IEnumerable<ArticleIndexModel>> GetArticlesAsync(int page = 1)
        {
            //throw new NotImplementedException();
             
            return Task.FromResult<IEnumerable<ArticleIndexModel>>(
            [
                new ArticleIndexModel
                {
                    Id = 1,
                    Title = "Sample Article",
                    Slug = "sample-article",
                     
                    CreatedDate = DateTime.UtcNow
                }
            ]);
        }

        public Task<ArticleDetailsModel> PostArticleAsync(ArticleDetailsModel article)
        {
            throw new NotImplementedException();
        }

        public Task<ArticleDetailsModel> PutArticleAsync(int id, ArticleDetailsModel article)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteArticleAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}

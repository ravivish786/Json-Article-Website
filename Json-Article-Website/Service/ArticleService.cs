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
            //throw new NotImplementedException();
             
            return Task.FromResult<IEnumerable<ArticleIndexModel>>(
            [
                new ArticleIndexModel
                {
                    Id = 1,
                    Title = "Sample Article",
                    Slug = "sample-article",
                    PublishedDate = DateTime.UtcNow,
                    Author = "Author Name",
                    Category = "Category Name",
                    Tags =
                    [
                        new() { Name = "Tag1" },
                        new() { Name = "Tag2" }
                    ],
                    ImageUrl = "https://example.com/image.jpg",
                    Summary = "This is a sample article summary.",
                    IsFeatured = true,
                    IsPublished = true,
                    IsDeleted = false,
                    IsModified = false,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.UtcNow
                }
            ]);
        }
    }
}

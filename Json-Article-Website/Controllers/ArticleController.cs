using Microsoft.AspNetCore.Mvc;

namespace Json_Article_Website.Controllers
{
    public class ArticleController : Controller
    {

        [Route("articles/{id}/{slug?}")]
        public async Task<IActionResult> Details(int id, string slug )
        {
            // Simulate fetching article details from a data source
            var article = new
            {
                Id = id,
                Title = $"Article {id}",
                Content = $"This is the content of article {id}."
            };
            return View(article);
        }

        [Route("articles")]
        public async Task<IActionResult> Articles(int page = 1)
        {
            // Simulate fetching articles from a data source
            var articles = new List<string>
            {
                "Article 1",
                "Article 2",
                "Article 3",
                "Article 4",
                "Article 5"
            };
            // Simulate pagination
            int pageSize = 2;
            var paginatedArticles = articles.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return View(paginatedArticles); 
        }
    }
}

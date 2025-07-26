using Json_Article_Website.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Json_Article_Website.Controllers
{
    public class ArticleController(IArticleService articleService, ILogger<ArticleController> logger) : Controller
    {
        private readonly ILogger<ArticleController> _logger = logger;
        private readonly IArticleService _articleService = articleService;
         

        [Route("articles/{id}/{slug?}")]
        public async Task<IActionResult> Details(int id, string slug )
        {
            var article = await _articleService.GetArticleDetailsAsync(id);


            if (article == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(slug) || !slug.Equals(article.Slug, StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToActionPermanent("Details", new { id = article.Id, slug = article.Slug });
            }

            return View(article);
        }

        [Route("articles")]
        public async Task<IActionResult> Articles(int page = 1)
        {
            var articles = await _articleService.GetArticlesAsync(page);
             
            return View(articles); 
        }
    }
}

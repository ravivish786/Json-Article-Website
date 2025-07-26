using System.Web;
using Json_Article_Website.Helper;
using Json_Article_Website.Interface;
using Json_Article_Website.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Json_Article_Website.Controllers
{
    public class AdminController (IArticleService articleService) : Controller
    {
        [Route("admin")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("new")]
        [HttpGet]
        public IActionResult New()
        {
            return View(new ArticleDetailsModel { });
        }


        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            var article = await articleService.GetArticleDetailsAsync(id);
            if (article == null)
            {
                return View("new", new ArticleDetailsModel { });
            }
            return View("new", article);
        }
        

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAsync(ArticleDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                model.Content = Sanatizer.SanitizeHtml(model.Content);
                model.Slug = UrlSlugger.GenerateSlug(model.Title);
                if (model.Id > 0)
                {
                    await articleService.PutArticleAsync(model.Id, model);
                }
                else
                { 
                    await articleService.PostArticleAsync(model);
                }
                return RedirectToActionPermanent("Details", "Article", new { id = model.Id, slug = model.Slug });
            }
            return View("new", model);
        }




         
    }
}

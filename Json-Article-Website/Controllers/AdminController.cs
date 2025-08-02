using System;
using Json_Article_Website.Extention;
using Json_Article_Website.Filter;
using Json_Article_Website.Helper;
using Json_Article_Website.Interface;
using Json_Article_Website.Models;
using Json_Article_Website.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Json_Article_Website.Controllers
{
    [BasicAuthentication]
    public class AdminController (IArticleService articleService) : Controller
    {
        [Route("admin")]
        public async Task<IActionResult> Index(int? page, CancellationToken cancellationToken)
        {
            var articles = await articleService.GetArticlesAsync(page, true, cancellationToken);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ArticleList", articles);
            }

            return View(articles);
        }


        [Route("new")]
        [HttpGet]
        public IActionResult New()
        {
            return View(new ArticleDetailsModel { });
        }


        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id, CancellationToken cancellationToken)
        {
            var article = await articleService.GetArticleDetailsAsync(id, cancellationToken);
            if (article == null)
            {
                return View("new", new ArticleDetailsModel { });
            }
            return View("new", article);
        }



        [Route("delete/{id}")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await articleService.DeleteArticleAsync(id, cancellationToken);

            if (!Request.IsAjaxRequest())
            {
                return RedirectToActionPermanent("Index", "Admin");
            }
            // return json response 
            return Json(new { success = true, message = "Article deleted successfully." });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ArticleDetailsModel model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                model.Content = Sanatizer.SanitizeHtml(model.Content);
                model.Slug = UrlSlugger.GenerateSlug(model.Title);
                 
                DateTime dateOnly = model.PublishedDate; // e.g., 2025-07-26 00:00:00
                DateTime _now = DateTime.Now;

                DateTime combined = dateOnly
                    .AddHours(_now.Hour)
                    .AddMinutes(_now.Minute)
                    .AddSeconds(_now.Second);

                model.PublishedDate = combined;
                 
                if (model.Id > 0)
                {
                    await articleService.PutArticleAsync(model.Id, model, cancellationToken);
                }
                else
                { 
                    await articleService.PostArticleAsync(model, cancellationToken);
                }
                return RedirectToActionPermanent("Details", "Article", new { id = model.Id, slug = model.Slug });
            }
            return View("new", model);
        }
         
    }
}

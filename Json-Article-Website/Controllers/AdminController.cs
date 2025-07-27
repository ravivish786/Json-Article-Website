using System;
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
        public async Task<IActionResult> Index(int? page = null)
        {
            var articles = await articleService.GetArticlesAsync(page);
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
        public async Task<IActionResult> EditAsync(int id)
        {
            var article = await articleService.GetArticleDetailsAsync(id);
            if (article == null)
            {
                return View("new", new ArticleDetailsModel { });
            }
            return View("new", article);
        }



        [Route("delete/{id}")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await articleService.DeleteArticleAsync(id);
            
            return RedirectToActionPermanent("Index", "Admin");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ArticleDetailsModel model)
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

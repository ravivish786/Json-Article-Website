using Microsoft.AspNetCore.Mvc;

namespace Json_Article_Website.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

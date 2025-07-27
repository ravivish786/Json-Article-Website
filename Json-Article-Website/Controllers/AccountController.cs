using Microsoft.AspNetCore.Mvc;

namespace Json_Article_Website.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        [Route("login")]
        public Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return Task.FromResult<IActionResult>(View());
        }

        [HttpPost]
        [Route("login")]
        public Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            // Here you would typically validate the user credentials
            // For simplicity, we are assuming the login is always successful
            if (ModelState.IsValid)
            {
                // Set authentication cookie or session here
                // Redirect to the return URL or default page
                return Task.FromResult<IActionResult>(Redirect(returnUrl ?? "/"));
            }
            ViewData["ReturnUrl"] = returnUrl;
            return Task.FromResult<IActionResult>(View());
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}

using Json_Article_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Json_Article_Website.Helper;

namespace Json_Article_Website.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        [Route("login")]
        public Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return Task.FromResult<IActionResult>(View(new LoginModel
            {
                ReturnUrl = returnUrl,
            }));
        }

        [HttpPost]
        [Route("login")]
        public Task<IActionResult> Login(LoginModel model)
        {
            // Here you would typically validate the user credentials
            // For simplicity, we are assuming the login is always successful
            if (ModelState.IsValid)
            {
                // valiadte from static data like admin:admin

                if (model.UserName == "admin" && model.Password == "admin")
                {
                    // Set the user as authenticated (this is just a placeholder, implement your own authentication logic)
                     
                    var cookie = new LoginSession
                    {
                        UserName = model.UserName,
                        IsAuthenticated = true,
                        LoginTime = DateTime.Now
                    };

                    CookieHelper.SetLoginCookie(Response, cookie, 7); // Set cookie for 7 days

                    // Redirect to the return URL or home page
                    var adminPage = Url.Action("Index", "Admin");
                    return Task.FromResult<IActionResult>(Redirect(model.ReturnUrl ?? adminPage ?? "/"));
                }
                 
            }
             
            return Task.FromResult<IActionResult>(View(model));
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}

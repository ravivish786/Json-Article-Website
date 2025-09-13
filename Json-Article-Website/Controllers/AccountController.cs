using Json_Article_Website.Models;
using Microsoft.AspNetCore.Mvc;
using Json_Article_Website.Helper;

namespace Json_Article_Website.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        [Route("login")]
        public Task<IActionResult> Login(string? returnUrl = null)
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

                    CookieHelper.SetLoginCookie(Response, cookie, 7*24*60*60); // Set cookie for 7 days

                    // Redirect to the return URL or home page
                    var adminPage = Url.Action("Index", "Admin");
                    return Task.FromResult<IActionResult>(Redirect(model.ReturnUrl ?? adminPage ?? "/"));
                }
                 
            }
            else
            {
                // return summery message like enter valid creadentia;
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please enter valid credentials.");
            }
             return Task.FromResult<IActionResult>(View(model));
        }

        [Route("logout")]
        public Task<IActionResult> Logout()
        {
            // Clear the authentication cookie
            CookieHelper.DeleteCookie(Response, CookieEnum.LoginSession.ToString());
            return Task.FromResult<IActionResult>(RedirectToAction("Index", "Home"));
        }

        [Route("profile")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

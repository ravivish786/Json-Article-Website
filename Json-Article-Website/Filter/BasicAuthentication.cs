using Json_Article_Website.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Json_Article_Website.Filter
{
    // custom authention filter by cookie 
    public class BasicAuthentication : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Check if the user is authenticated
            var loginSession = context.HttpContext.Request.GetLoginCookie();
            if (loginSession == null || !loginSession.IsAuthenticated)
            {
                // If not authenticated, redirect to login page
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = context.HttpContext.Request.Path });
            }
            await Task.CompletedTask;
        }
    }   
}

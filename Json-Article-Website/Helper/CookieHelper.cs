using System.Text.Json;
using AngleSharp.Io;
using Json_Article_Website.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Json_Article_Website.Helper
{
    public enum CookieEnum
    {
        LoginSession
    }

    public static class CookieHelper
    {
        public static void SetCookie(this HttpResponse response, string key, string value, int expireSeconds = 7)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.Now.AddSeconds(expireSeconds),

            };
             
            response.Cookies.Append(key, value, cookieOptions);
        }

        public static void SetCookie<T>(this HttpResponse response, string key, T value, int expireSeconds = 7)
        {
            var cookieValue = Convert.ToString(value) ;
            SetCookie(response, key, cookieValue, expireSeconds);
        }

        public static string GetCookie(this HttpRequest request, string key)
        {
            return request.Cookies.TryGetValue(key, out var value) ? value : string.Empty;
        }

        public static T GetCookie<T>(this HttpRequest request, string key)
        {
            var value = GetCookie(request, key);

            return string.IsNullOrEmpty(value) 
                ? default : (T)Convert.ChangeType(value, typeof(T));
        }

        public static void DeleteCookie(this HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
        }



        #region Set Login Cookie

        public static void SetLoginCookie(this HttpResponse response, LoginSession loginSession, int expireSeconds = 7)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(loginSession);
            var cookieValue = Convert.ToBase64String(bytes);
            SetCookie(response, CookieEnum.LoginSession.ToString(), cookieValue, expireSeconds);
        }

        public static LoginSession GetLoginCookie(this HttpRequest request)
        {
           return GetLoginSession(request);
        }

        public static LoginSession GetLoginCookie(this HtmlHelper htmlHelper)
        {
            var request = htmlHelper.ViewContext.HttpContext.Request;
            return GetLoginSession(request);
        }


        private static LoginSession GetLoginSession(HttpRequest request)
        {
            var defaultLogin = new LoginSession();

            var cookieValue = GetCookie(request, CookieEnum.LoginSession.ToString());

            if (string.IsNullOrEmpty(cookieValue))
            {
                return defaultLogin;
            }

            var bytes = Convert.FromBase64String(cookieValue);
            if (bytes.Length == 0)
            {
                return defaultLogin;
            }
            // Deserialize the cookie value back to LoginSession object

            var loginSession = JsonSerializer.Deserialize<LoginSession>(bytes);

            return loginSession ?? defaultLogin;
        }

        #endregion
    }
}

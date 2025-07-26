using Ganss.Xss;

namespace Json_Article_Website.Helper
{
    public static class Sanatizer
    {
        private static readonly HtmlSanitizer sanitizer = new();

        public static string SanitizeHtml(string htmlContent)
        {
            //sanitizer.AllowedTags.Add("img");
            //sanitizer.AllowedAttributes.Add("src");
            //sanitizer.AllowedAttributes.Add("alt");
            //sanitizer.AllowedAttributes.Add("title");
            return sanitizer.Sanitize(htmlContent);
        }
    }
}

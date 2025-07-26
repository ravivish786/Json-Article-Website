using System.Text.RegularExpressions;
using System.Text;

namespace Json_Article_Website.Helper
{
    public static class UrlSlugger
    {
        public static string GenerateSlug(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Convert to lowercase
            input = input.ToLowerInvariant();

            // Remove diacritics (accents) from Latin characters
            input = RemoveDiacritics(input);

            // Replace spaces and invalid characters with hyphens
            input = Regex.Replace(input, @"[^a-z0-9\s-]", ""); // Allow only alphanumeric, spaces, and hyphens
            input = Regex.Replace(input, @"\s+", "-").Trim();  // Replace spaces with hyphens
            input = Regex.Replace(input, @"-+", "-");         // Replace multiple hyphens with a single one

            return input;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

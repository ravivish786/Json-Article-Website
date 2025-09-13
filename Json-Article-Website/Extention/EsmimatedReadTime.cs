using System.Text.RegularExpressions;

namespace Json_Article_Website.Extention
{
    public static class EsmimatedReadTime
    {
        // content can take html , markdown, or plain text

        /// <summary>
        /// show reading time in minutes for the given content
        /// </summary>
        public static int GetEstimatedReadTime(this string content, int wordsPerMinute = 200)
        {
            if (string.IsNullOrEmpty(content))
            {
                return 0;
            }

            // if content is containing HTML tags, we can strip them out to get plain text
            if (content.Contains('<') && content.Contains('>'))
            {
                // Simple HTML tag removal
                content = Regex.Replace(content, "<.*?>", string.Empty);
            }

            // Average reading speed is about 200-250 words per minute
            string[] words = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int wordCount = words.Length;
            // Calculate estimated reading time in minutes
            int estimatedTime = (int)Math.Ceiling((double)wordCount / wordsPerMinute);
            return estimatedTime;
        }

        public static string ToEstimatedReadTimeString(this int minutes)
        {
            if (minutes > 60)
            {
                int hours = minutes / 60;
                int remainingMinutes = minutes % 60;
                return remainingMinutes > 0 ? $"{hours}hr {remainingMinutes}min read" : $"{hours}hr read";
            }

            return $"{minutes}min read";
        }
    }
}

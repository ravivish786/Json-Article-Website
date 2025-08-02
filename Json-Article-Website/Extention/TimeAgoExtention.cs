namespace Json_Article_Website.Extention
{
    public static class TimeAgoExtention
    {
        public static string TimeAgo(this DateTime? date)
        {
            if (date == null)
                return "unknown";

            var time = date.Value;
            var now = DateTime.Now;
            var span = now - time;

            if (span.TotalSeconds < 60)
                return "just now";

            if (span.TotalMinutes < 60)
                return $"{(int)span.TotalMinutes} minute{(span.TotalMinutes < 2 ? "" : "s")} ago";

            if (span.TotalHours < 24)
                return $"{(int)span.TotalHours} hour{(span.TotalHours < 2 ? "" : "s")} ago";

            if (span.TotalDays < 2)
                return "yesterday";

            if (span.TotalDays < 30)
                return $"{(int)span.TotalDays} day{(span.TotalDays < 2 ? "" : "s")} ago";

            if (span.TotalDays < 365)
            {
                int months = (int)(span.TotalDays / 30);
                return $"{months} month{(months < 2 ? "" : "s")} ago";
            }

            int years = (int)(span.TotalDays / 365);
            return $"{years} year{(years < 2 ? "" : "s")} ago";
        }
    }

}

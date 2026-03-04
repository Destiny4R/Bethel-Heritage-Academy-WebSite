using System.Text.RegularExpressions;

namespace Bethel_Heritage_Academy_WebSite.Services
{
    public static class ST
    {
        public static string GetRelativeTime(DateTime dateTime)
        {
            var now = DateTime.UtcNow;
            var input = dateTime.Kind == DateTimeKind.Utc
                ? dateTime
                : dateTime.ToUniversalTime();

            var timeSpan = now - input;

            if (timeSpan.TotalSeconds < 60)
                return $"{(int)timeSpan.TotalSeconds} second(s)";

            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} minute(s)";

            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} hour(s)";

            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} day(s)";

            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)} week(s)";

            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} month(s)";

            return $"{(int)(timeSpan.TotalDays / 365)} year(s)";
        }

        public static string TruncateHtml(string htmlContent, int maxLength = 150)
        {
            if (string.IsNullOrEmpty(htmlContent))
                return string.Empty;

            // Remove HTML tags to get plain text
            string plainText = Regex.Replace(htmlContent, "<.*?>", string.Empty);

            // Decode HTML entities (like &nbsp;, &amp;, etc.)
            plainText = System.Net.WebUtility.HtmlDecode(plainText);

            // Trim whitespace
            plainText = plainText.Trim();

            // If text is shorter than max length, return as is
            if (plainText.Length <= maxLength)
                return plainText;

            // Truncate to maxLength and add ellipsis
            string truncated = plainText.Substring(0, maxLength);

            // Try to truncate at last space to avoid cutting words
            int lastSpace = truncated.LastIndexOf(' ');
            if (lastSpace > maxLength - 20) // Only use last space if it's not too far back
            {
                truncated = truncated.Substring(0, lastSpace);
            }

            return truncated.TrimEnd() + "...";
        }
    }
}

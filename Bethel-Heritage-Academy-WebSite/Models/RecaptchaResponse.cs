namespace Bethel_Heritage_Academy_WebSite.Models
{
    public class RecaptchaResponse
    {
        public bool success { get; set; }
        public string challenge_ts { get; set; }
        public string hostname { get; set; }
        public string[] error_codes { get; set; }
    }
}

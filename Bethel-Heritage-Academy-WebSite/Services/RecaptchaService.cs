using System.Text.Json;
using Bethel_Heritage_Academy_WebSite.Models;

namespace Bethel_Heritage_Academy_WebSite.Services
{
    public interface IRecaptchaService
    {
        Task<bool> ValidateRecaptchaAsync(string recaptchaResponse);
    }

    public class RecaptchaService : IRecaptchaService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public RecaptchaService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ValidateRecaptchaAsync(string recaptchaResponse)
        {
            if (string.IsNullOrEmpty(recaptchaResponse))
                return false;

            var secretKey = _configuration["ReCaptcha:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                // If no secret key is configured, return true for development
                return true;
            }

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}",
                null);

            if (!response.IsSuccessStatusCode)
                return false;

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RecaptchaResponse>(jsonString);

            return result?.success ?? false;
        }
    }
}

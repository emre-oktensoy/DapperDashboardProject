namespace DapperDashboardProject.Repositories.Concrete
{
    using DapperDashboardProject.Models;
    using System.Net.Http;
    using System.Text.Json;

    public class GoogleNewsService
    {
        private readonly HttpClient _httpClient;

        public GoogleNewsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

      

        public async Task<List<GoogleNewsItem>> GetNewsAsync(string keyword)
        {
            var url = $"https://google-news13.p.rapidapi.com/search?keyword={Uri.EscapeDataString(keyword)}&lr=tr-TR";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
        {
            { "x-rapidapi-host", "google-news13.p.rapidapi.com" },
            { "x-rapidapi-key", "Api Key" }
        }
            };

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<GoogleNewsApiResponse>(body, options);

            return result?.Items ?? new List<GoogleNewsItem>();
        }

    }

}

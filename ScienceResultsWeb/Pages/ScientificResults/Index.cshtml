using Microsoft.AspNetCore.Mvc.RazorPages;
using ScienceResultsWeb.Models;
using System.Text.Json;

namespace ScienceResultsWeb.Pages.ScientificResults
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ScientificResultDto> Results { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5123/api/ScientificResults");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Results = JsonSerializer.Deserialize<List<ScientificResultDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new();
            }
        }
    }
}
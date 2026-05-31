using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace ScienceResultsWeb.Pages
{
    public class PublishersModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PublishersModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<PublisherDto> Publishers { get; set; } = new();

        [BindProperty]
        public PublisherDto NewPublisher { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5123/api/Publishers");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Publishers = JsonSerializer.Deserialize<List<PublisherDto>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var json = JsonSerializer.Serialize(NewPublisher);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync("http://localhost:5123/api/Publishers", content);

            return RedirectToPage();
        }

        public class PublisherDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
        }
    }
}
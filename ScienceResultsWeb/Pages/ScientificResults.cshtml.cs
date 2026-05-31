using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace ScienceResultsWeb.Pages
{
    public class ScientificResultsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ScientificResultsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ScientificResultDto> Results { get; set; } = new();

        [BindProperty]
        public ScientificResultDto NewResult { get; set; } = new();

        public string Search { get; set; } = "";

        public async Task OnGetAsync(string? search)
        {
            Search = search ?? "";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5123/api/ScientificResults");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                Results = JsonSerializer.Deserialize<List<ScientificResultDto>>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new();

                if (!string.IsNullOrWhiteSpace(Search))
                {
                    Results = Results
                        .Where(x =>
                            x.Title.Contains(Search, StringComparison.OrdinalIgnoreCase)
                            || x.DOI.Contains(Search, StringComparison.OrdinalIgnoreCase)
                            || x.Year.ToString().Contains(Search))
                        .ToList();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var json = JsonSerializer.Serialize(NewResult);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync("http://localhost:5123/api/ScientificResults", content);

            return RedirectToPage();
        }

        public class ScientificResultDto
        {
            public int Id { get; set; }
            public string Title { get; set; } = "";
            public int Year { get; set; }
            public string DOI { get; set; } = "";
            public string Url { get; set; } = "";
            public int ResultTypeId { get; set; }
        }
    }
}
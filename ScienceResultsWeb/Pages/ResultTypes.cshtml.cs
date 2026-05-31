using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace ScienceResultsWeb.Pages
{
    public class ResultTypesModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ResultTypesModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ResultTypeDto> ResultTypes { get; set; } = new();

        [BindProperty]
        public ResultTypeDto NewResultType { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5123/api/ResultTypes");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                ResultTypes = JsonSerializer.Deserialize<List<ResultTypeDto>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var json = JsonSerializer.Serialize(NewResultType);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync("http://localhost:5123/api/ResultTypes", content);

            return RedirectToPage();
        }

        public class ResultTypeDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
        }
    }
}
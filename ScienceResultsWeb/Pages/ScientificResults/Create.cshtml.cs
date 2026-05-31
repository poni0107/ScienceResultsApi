using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScienceResultsWeb.Models;
using System.Text;
using System.Text.Json;

namespace ScienceResultsWeb.Pages.ScientificResults
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ScientificResultDto ScientificResult { get; set; } = new();

        public List<SelectListItem> ResultTypeOptions { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadResultTypesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadResultTypesAsync();

            var client = _httpClientFactory.CreateClient();

            var json = JsonSerializer.Serialize(new
            {
                title = ScientificResult.Title,
                year = ScientificResult.Year,
                doi = ScientificResult.DOI,
                resultTypeId = ScientificResult.ResultTypeId
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5123/api/ScientificResults", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to save scientific result.");
                return Page();
            }

            return RedirectToPage("Index");
        }

        private async Task LoadResultTypesAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5123/api/ResultTypes");

            if (!response.IsSuccessStatusCode) return;

            var json = await response.Content.ReadAsStringAsync();
            var resultTypes = JsonSerializer.Deserialize<List<ResultTypeDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();

            ResultTypeOptions = resultTypes
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
                .ToList();
        }
    }
}
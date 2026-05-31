using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ScienceResultsWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public int AuthorsCount { get; set; }
        public int ResultTypesCount { get; set; }
        public int ScientificResultsCount { get; set; }
        public int PublishersCount { get; set; }

        public List<ScientificResultDto> LatestResults { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var authors = await GetList<AuthorDto>(client, "http://localhost:5123/api/Authors");
            var resultTypes = await GetList<ResultTypeDto>(client, "http://localhost:5123/api/ResultTypes");
            var scientificResults = await GetList<ScientificResultDto>(client, "http://localhost:5123/api/ScientificResults");
            var publishers = await GetList<PublisherDto>(client, "http://localhost:5123/api/Publishers");

            AuthorsCount = authors.Count;
            ResultTypesCount = resultTypes.Count;
            ScientificResultsCount = scientificResults.Count;
            PublishersCount = publishers.Count;

            LatestResults = scientificResults
                .OrderByDescending(x => x.Year)
                .Take(5)
                .ToList();
        }

        private static async Task<List<T>> GetList<T>(HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return new List<T>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<T>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<T>();
        }

        public class AuthorDto
        {
            public int Id { get; set; }
        }

        public class ResultTypeDto
        {
            public int Id { get; set; }
        }

        public class PublisherDto
        {
            public int Id { get; set; }
        }

        public class ScientificResultDto
        {
            public int Id { get; set; }
            public string Title { get; set; } = "";
            public int Year { get; set; }
            public string DOI { get; set; } = "";
        }
    }
}
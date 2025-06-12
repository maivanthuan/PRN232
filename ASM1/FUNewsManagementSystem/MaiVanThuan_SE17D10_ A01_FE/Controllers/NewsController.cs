using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace MaiVanThuan_SE17D10__A01_FE.Controllers
{
    
        public class NewsController : Controller
        {
            private readonly HttpClient _httpClient;

            public NewsController(IHttpClientFactory httpClientFactory)
            {
                _httpClient = httpClientFactory.CreateClient();
                _httpClient.BaseAddress = new Uri("https://localhost:7080/api/");
            }

            public async Task<IActionResult> Index()
            {
                var response = await _httpClient.GetAsync("news/active");
                if (!response.IsSuccessStatusCode) return View("Error");

                var json = await response.Content.ReadAsStringAsync();
                var newsList = JsonSerializer.Deserialize<List<NewsViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(newsList ?? new List<NewsViewModel>());
        }
        }

        public class NewsViewModel
        {
            public string NewsTitle { get; set; } = string.Empty;
            public string NewsContent { get; set; } = string.Empty;
            public string AuthorName { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; }
        }
    }


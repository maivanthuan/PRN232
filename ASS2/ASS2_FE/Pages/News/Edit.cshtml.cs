using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ASS2_FE.Pages.News
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _config;

        public EditModel(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _config = config;
        }

        [BindProperty]
        public NewsDto Item { get; set; }

        [BindProperty(SupportsGet = true)]
        public string id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(_config["ApiBase"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var res = await client.GetAsync($"api/NewsArticles/{id}");
            if (!res.IsSuccessStatusCode) return NotFound();

            var json = await res.Content.ReadAsStringAsync();
            Item = JsonSerializer.Deserialize<NewsDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(_config["ApiBase"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Item.NewsArticleId = id;

            var content = new StringContent(JsonSerializer.Serialize(Item), System.Text.Encoding.UTF8, "application/json");
            var res = await client.PutAsync($"api/NewsArticles/{id}", content);

            if (res.IsSuccessStatusCode) return RedirectToPage("/News/All");
            return Page(); // hoặc hiển thị lỗi nếu muốn
        }

        public class NewsDto
        {
            public string NewsArticleId { get; set; }
            public string NewsTitle { get; set; }
            public string Headline { get; set; }
            public bool? NewsStatus { get; set; }
        }
    }

}

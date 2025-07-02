using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ASS2_FE.Pages.News
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _config;

        public DeleteModel(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _config = config;
        }

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
            if (Item == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Login");

            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(_config["ApiBase"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var resGet = await client.GetAsync($"api/NewsArticles/{id}");
            if (resGet.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                resGet.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToPage("/Login");
            }
            if (!resGet.IsSuccessStatusCode) return NotFound();

            var json = await resGet.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Item = JsonSerializer.Deserialize<NewsDto>(json, options);

            if (Item == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy bài viết để xoá.");
                return Page();
            }

            Item.NewsStatus = false;

            var content = new StringContent(JsonSerializer.Serialize(Item), Encoding.UTF8, "application/json");
            var resPut = await client.PutAsync($"api/NewsArticles/{id}", content);

            if (resPut.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                resPut.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToPage("/Login");
            }

            if (resPut.IsSuccessStatusCode)
            {
                TempData["Message"] = "🗑️ Bài viết đã được chuyển sang trạng thái Nháp.";
                return RedirectToPage("/News/All");
            }

            ModelState.AddModelError(string.Empty, "Không thể cập nhật trạng thái bài viết.");
            return Page();
        }

        public class NewsDto
        {
            public string NewsTitle { get; set; }
            public string Headline { get; set; }
            public bool? NewsStatus { get; set; }
        }
    }
}
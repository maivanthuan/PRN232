using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASS2_FE.Pages.News
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _config;

        public CreateModel(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _config = config;
        }

        [BindProperty]
        public NewsDto Item { get; set; }

        [BindProperty]
        public List<int> SelectedTags { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public List<SelectListItem> CategoryOptions { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<TagDto> AllTags { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(_config["ApiBase"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await LoadCategoriesAndTags(client);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/Login");

            var accountName = HttpContext.Session.GetString("AccountName");

            Item.CreatedBy = accountName;
            Item.CreatedDate = DateTime.Now;
            Item.TagIds = SelectedTags;

            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(_config["ApiBase"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonSerializer.Serialize(Item), Encoding.UTF8, "application/json");
            var res = await client.PostAsync("api/NewsArticles", content);

            if (res.IsSuccessStatusCode)
                return RedirectToPage("/News/All");

            ModelState.AddModelError(string.Empty, "Không thể tạo bài viết.");
            await LoadCategoriesAndTags(client);
            return Page();
        }

        private async Task LoadCategoriesAndTags(HttpClient client)
        {
            var resCategory = await client.GetAsync("odata/cates");
            if (resCategory.IsSuccessStatusCode)
            {
                var json = await resCategory.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<CategoryDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                CategoryOptions = categories.Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.CategoryId.ToString() // hoặc giữ nguyên nếu là string
                }).ToList();
            }

            var resTag = await client.GetAsync("odata/Tags");
            if (resTag.IsSuccessStatusCode)
            {
                var json = await resTag.Content.ReadAsStringAsync();
                AllTags = JsonSerializer.Deserialize<List<TagDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        public class NewsDto
        {
            public string NewsTitle { get; set; }
            public string Headline { get; set; }
            public string NewsContent { get; set; }
            public bool NewsStatus { get; set; }
            public string CategoryId { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public List<int> TagIds { get; set; }
        }

        public class TagDto
        {
            public int TagId { get; set; }
            public string TagName { get; set; }
        }

        public class CategoryDto
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
        }
    }
}
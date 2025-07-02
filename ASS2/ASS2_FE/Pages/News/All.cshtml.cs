using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASS2_FE.Pages.News
{
    public class AllModel : PageModel
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _config;

        public AllModel(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _config = config;
        }

        public List<NewsDto>? NewsList { get; set; }

        public async Task OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                Response.Redirect("/Login");
                return;
            }

            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(_config["ApiBase"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var res = await client.GetAsync("odata/News?$expand=Category,CreatedBy,Tags");

            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();

                NewsList = JsonSerializer.Deserialize<List<NewsDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


            }
        }

        public class NewsDto
        {
            public string NewsArticleId { get; set; }
            public string? NewsTitle { get; set; }
            public string Headline { get; set; }
            public DateTime? CreatedDate { get; set; }
            public bool? NewsStatus { get; set; }

            public CategoryDto? Category { get; set; }
            public SystemAccountDto? CreatedBy { get; set; }

            // 👇 Thay vì List<TagDto>
            public TagsWrapper? Tags { get; set; }
        }

        public class TagsWrapper
        {
            [JsonPropertyName("$values")]
            public List<TagDto> Values { get; set; }
        }

        public class CategoryDto { public string CategoryName { get; set; } }
        public class SystemAccountDto { public string AccountName { get; set; } }
        public class TagDto { public string TagName { get; set; } }


    }
}


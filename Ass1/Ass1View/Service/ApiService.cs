using System.Text.Json;

namespace Ass1View.Service
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7019/odata/");
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("Categories");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(content);
            return data.GetProperty("value").EnumerateArray()
                .Select(x => new Category
                {
                    CategoryId = x.GetProperty("CategoryId").GetInt16(),
                    CategoryName = x.GetProperty("CategoryName").GetString(),
                    CategoryDesciption = x.GetProperty("CategoryDesciption").GetString(),
                    ParentCategoryId = x.GetProperty("ParentCategoryId").GetInt16(),
                    IsActive = x.GetProperty("IsActive").GetBoolean(),
                    // Navigation properties (không deserialize trực tiếp ở đây, để null hoặc xử lý riêng)
                    InverseParentCategory = new List<Category>(),
                    NewsArticles = new List<NewsArticle>(),
                    ParentCategory = null
                }).ToList();
        }

        public async Task<Category> GetCategoryByIdAsync(short id)
        {
            var response = await _httpClient.GetAsync($"Categories/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Category>(content);
        }

        public async Task<List<NewsArticle>> GetNewsArticlesAsync()
        {
            var response = await _httpClient.GetAsync("NewsArticles");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(content);
            return data.GetProperty("value").EnumerateArray()
                .Select(x => new NewsArticle
                {
                    NewsArticleId = x.GetProperty("NewsArticleId").GetString(),
                    NewsTitle = x.GetProperty("NewsTitle").GetString(),
                    Headline = x.GetProperty("Headline").GetString(),
                    CreatedDate = x.GetProperty("CreatedDate").GetDateTime(),
                    NewsContent = x.GetProperty("NewsContent").GetString(),
                    NewsSource = x.GetProperty("NewsSource").GetString(),
                    CategoryId = x.GetProperty("CategoryId").GetInt16(),
                    NewsStatus = x.GetProperty("NewsStatus").GetBoolean(),
                    CreatedById = x.GetProperty("CreatedById").GetInt16(),
                    UpdatedById = x.GetProperty("UpdatedById").GetInt16(),
                    ModifiedDate = x.GetProperty("ModifiedDate").GetDateTime(),
                    // Navigation properties
                    Category = null,
                    CreatedBy = null,
                    Tags = new List<Tag>()
                }).ToList();
        }

        public async Task<NewsArticle> GetNewsArticleByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"NewsArticles/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<NewsArticle>(content);
        }

        public async Task<List<SystemAccount>> GetSystemAccountsAsync()
        {
            var response = await _httpClient.GetAsync("SystemAccounts");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(content);
            return data.GetProperty("value").EnumerateArray()
                .Select(x => new SystemAccount
                {
                    AccountId = x.GetProperty("AccountId").GetInt16(),
                    AccountName = x.GetProperty("AccountName").GetString(),
                    AccountEmail = x.GetProperty("AccountEmail").GetString(),
                    AccountRole = x.GetProperty("AccountRole").GetInt32(),
                    AccountPassword = x.GetProperty("AccountPassword").GetString(),
                    // Navigation properties
                    NewsArticles = new List<NewsArticle>(),
                    CreatedNewsArticles = new List<NewsArticle>()
                }).ToList();
        }

        public async Task<SystemAccount> GetSystemAccountByIdAsync(short id)
        {
            var response = await _httpClient.GetAsync($"SystemAccounts/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<SystemAccount>(content);
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            var response = await _httpClient.GetAsync("Tags");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(content);
            return data.GetProperty("value").EnumerateArray()
                .Select(x => new Tag
                {
                    TagId = x.GetProperty("TagId").GetInt32(),
                    TagName = x.GetProperty("TagName").GetString(),
                    Note = x.GetProperty("Note").GetString(),
                    // Navigation property
                    NewsArticles = new List<NewsArticle>()
                }).ToList();
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Tags/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Tag>(content);
        }
    }

    public class Category
    {
        public short CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryDesciption { get; set; } = null!;
        public short? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
        public virtual Category? ParentCategory { get; set; }
    }

    public class NewsArticle
    {
        public string NewsArticleId { get; set; } = null!;
        public string? NewsTitle { get; set; }
        public string Headline { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public short? CategoryId { get; set; }
        public bool? NewsStatus { get; set; }
        public short? CreatedById { get; set; }
        public short? UpdatedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual Category? Category { get; set; }
        public virtual SystemAccount? CreatedBy { get; set; }
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }

    public class SystemAccount
    {
        public short AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public int? AccountRole { get; set; }
        public string? AccountPassword { get; set; }
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
        public virtual ICollection<NewsArticle> CreatedNewsArticles { get; set; } = new List<NewsArticle>();
    }

    public class Tag
    {
        public int TagId { get; set; }
        public string? TagName { get; set; }
        public string? Note { get; set; }
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}

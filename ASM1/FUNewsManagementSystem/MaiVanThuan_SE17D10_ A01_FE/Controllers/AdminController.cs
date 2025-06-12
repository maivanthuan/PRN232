using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace MaiVanThuan_SE17D10__A01_FE.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7080/api/");
        }

        public async Task<IActionResult> AccountList(string? keyword, int page = 1, int pageSize = 5)
        {
            var response = await _httpClient.GetAsync("account");
            if (!response.IsSuccessStatusCode) return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<AccountViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!string.IsNullOrWhiteSpace(keyword))
                data = data.Where(a => a.AccountName.Contains(keyword, StringComparison.OrdinalIgnoreCase) || a.AccountEmail.Contains(keyword)).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(data.Count / (double)pageSize);
            ViewBag.Keyword = keyword;

            return View(data.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(short id)
        {
            var response = await _httpClient.DeleteAsync($"account/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Xóa tài khoản thành công.";
            }
            else
            {
                TempData["Error"] = "Không thể xóa tài khoản đã có bài viết.";
            }
            return RedirectToAction("AccountList");
        }
        public async Task<IActionResult> Statistic(DateTime? start, DateTime? end, string? keyword = null, int page = 1, int pageSize = 4)
        {
            if (!start.HasValue) start = DateTime.Today.AddDays(-30);
            if (!end.HasValue) end = DateTime.Today;

            var response = await _httpClient.GetAsync($"statistic/news?start={start:yyyy-MM-dd}&end={end:yyyy-MM-dd}");
            if (!response.IsSuccessStatusCode) return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<NewsStatisticViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!string.IsNullOrWhiteSpace(keyword))
                data = data.Where(x => x.NewsTitle.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.Keyword = keyword;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(data.Count / (double)pageSize);

            return View(data.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonContent.Create(model);
            var response = await _httpClient.PostAsync("account", json);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Tạo tài khoản thành công.";
                return RedirectToAction("AccountList");
            }

            TempData["Error"] = "Tạo tài khoản thất bại.";
            return View(model);
        }
    }

    public class AccountViewModel
    {
        public short AccountId { get; set; }

        [Required]
        public string AccountName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string AccountEmail { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string AccountPassword { get; set; } = string.Empty; // ✅ giống BE

        [Range(1, 2)] // ✅ match với backend
        public int AccountRole { get; set; }
    }


    public class NewsStatisticViewModel
    {
        public string NewsTitle { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}

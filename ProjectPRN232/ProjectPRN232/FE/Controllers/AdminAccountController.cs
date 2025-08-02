using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FE.Models;

namespace FE.Controllers
{
    public class AdminAccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminAccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7089/"); // Backend port
        }

        // GET: List all accounts with search by Name and client-side pagination
        [HttpGet]
        public async Task<IActionResult> Index(string search = null, int page = 1, int pageSize = 5) // Sửa: Thêm page và pageSize cho phân trang
        {
            // Sửa: Validate page và pageSize
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            // Sửa: Xây dựng query chỉ với filter (lấy full list, không pagination ở backend)
            var baseQuery = "api/account";
            var query = !string.IsNullOrEmpty(search)
                ? $"{baseQuery}?$filter=Name ne null and contains(tolower(Name),'{search.ToLower()}')"
                : baseQuery;

            // Get full accounts list
            var response = await _httpClient.GetAsync(query);
            List<AccountViewModel> accounts = new List<AccountViewModel>();
            if (response.IsSuccessStatusCode)
            {
                accounts = await response.Content.ReadFromJsonAsync<List<AccountViewModel>>() ?? new List<AccountViewModel>();
            }

            // Sửa: Phân trang thủ công ở client-side
            int totalCount = accounts.Count;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var paginatedAccounts = accounts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Sửa: Truyền dữ liệu phân trang vào ViewBag
            ViewBag.Search = search;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;

            return View(paginatedAccounts); // Sửa: Trả list đã phân trang
        }

        // POST: Toggle Block
        [HttpPost]
        public async Task<IActionResult> ToggleBlock(int id, string search, int page = 1) // Sửa: Thêm search và page để giữ trạng thái
        {
            var response = await _httpClient.PutAsync($"api/account/toggle-block/{id}", null);
            if (!response.IsSuccessStatusCode) return BadRequest("Toggle failed.");

            // Sửa: Redirect giữ search và page
            return RedirectToAction(nameof(Index), new { search, page });
        }
    }
}
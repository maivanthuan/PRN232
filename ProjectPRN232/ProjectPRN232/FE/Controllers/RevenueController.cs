// Trong file: FE/Controllers/RevenueController.cs
using System.Text.Json;
using FE.Models; // Namespace chứa các model bạn vừa tạo
using Microsoft.AspNetCore.Mvc;
using static FE.Models.RevenueViewModel;

namespace FE.Controllers
{
    public class RevenueController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        // Đặt URL của API backend vào đây
        private readonly string _apiBaseUrl = "https://localhost:7089/api";

        public RevenueController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var revenueData = new List<YearlyRevenueDTO>();

            try
            {
                // Gọi đến API để lấy dữ liệu doanh thu
                var response = await client.GetAsync($"{_apiBaseUrl}/TotalInvoicePitch/Revenue/Breakdown");

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    revenueData = await JsonSerializer.DeserializeAsync<List<YearlyRevenueDTO>>(
                        responseStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<YearlyRevenueDTO>();
                }
                else
                {
                    // Nếu gọi API thất bại, hiển thị lỗi
                    ViewBag.ErrorMessage = $"Lỗi gọi API: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi hệ thống (ví dụ: API server không chạy)
                ViewBag.ErrorMessage = $"Không thể kết nối đến API. Lỗi: {ex.Message}";
            }

            // Truyền dữ liệu doanh thu đã lấy được sang cho View
            return View(revenueData);
        }
    }
}
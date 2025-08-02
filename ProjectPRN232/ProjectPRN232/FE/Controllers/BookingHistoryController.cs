using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using FE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    [Authorize] // Bắt buộc người dùng phải đăng nhập mới vào được trang này
    public class BookingHistoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7089/api"; // Thay bằng URL API của bạn

        public BookingHistoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy UserId của người dùng đang đăng nhập từ Claims, an toàn hơn
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                // Nếu không tìm thấy UserId, chuyển hướng về trang đăng nhập
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();
            var invoices = new List<AdminInvoiceViewModel>();

            try
            {
                // Đọc token từ Cookie, khớp với AuthController
                var token = HttpContext.Request.Cookies["JwtToken"];
                if (string.IsNullOrEmpty(token))
                {
                    ViewBag.ErrorMessage = "Phiên đăng nhập đã hết hạn hoặc không hợp lệ. Vui lòng đăng nhập lại.";
                    return View(invoices); // Trả về view rỗng với thông báo lỗi
                }

                // Gắn token vào header của mỗi request
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Gọi API để lấy lịch sử đặt sân của người dùng
                var response = await client.GetAsync($"{_apiBaseUrl}/TotalInvoicePitch/byuser/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();

                    // SỬA LỖI Ở ĐÂY: Đọc trực tiếp thành List vì API trả về mảng JSON đơn giản
                    invoices = await JsonSerializer.DeserializeAsync<List<AdminInvoiceViewModel>>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AdminInvoiceViewModel>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.Message = "Bạn chưa có lịch sử đặt sân nào.";
                }
                else
                {
                    ViewBag.ErrorMessage = "Không thể tải lịch sử đặt sân từ server.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Lỗi kết nối đến máy chủ API: {ex.Message}";
            }

            return View(invoices);
        }
    }
}
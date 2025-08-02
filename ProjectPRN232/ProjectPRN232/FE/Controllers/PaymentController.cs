using FE.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using System.Diagnostics;

using System.Net.Http.Headers; // Thêm namespace này
using System.Security.Claims;
using System.Text;
using System.Text.Json; // Thêm namespace này

namespace FE.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7089/api"; // Đảm bảo đúng URL API của bạn

        public PaymentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public IActionResult Index([FromForm] PaymentViewModel model, [FromForm] string[] selectedCombinedSlots)
        {
            // Tách chuỗi kết hợp "ID|Time" thành 2 danh sách riêng biệt
            if (selectedCombinedSlots != null)
            {
                foreach (var combined in selectedCombinedSlots)
                {
                    var parts = combined.Split('|');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int id))
                    {
                        model.SelectedSlots.Add(id);
                        model.SelectedSlotTimes.Add(parts[1]);
                    }
                }
            }

            Debug.WriteLine("--- Dữ liệu nhận được tại trang thanh toán ---\n");
            Debug.WriteLine($"Sân bóng: {model.PitchId}");
            Debug.WriteLine($"Ngày đặt: {model.SelectedDate.ToShortDateString()}");
            Debug.WriteLine($"Tổng tiền: {model.TotalPrice}");
            Debug.WriteLine($"Các ID đã chọn: {string.Join(", ", model.SelectedSlots)}");
            Debug.WriteLine($"Các Time đã chọn: {string.Join(", ", model.SelectedSlotTimes)}\n");

            return View(model);
        }

        // Action mới để khởi tạo thanh toán VNPay
        [HttpPost]
        public async Task<IActionResult> InitiateVnPayPayment([FromForm] PaymentViewModel model)
        {
            var client = _httpClientFactory.CreateClient();

            // Lấy token từ cookie
            var token = HttpContext.Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Bạn cần đăng nhập để thực hiện thanh toán.");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Tạo đối tượng request để gửi đến API backend
            var paymentRequestDto = new PaymentRequestDTO // Đây là DTO ở Backend
            {
                PitchId = model.PitchId,
                SelectedDate = model.SelectedDate,
                SelectedSlots = model.SelectedSlots,
                Amount = model.TotalPrice // Số tiền sẽ được gửi cho VNPay
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(paymentRequestDto),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await client.PostAsync($"{_apiBaseUrl}/TotalInvoicePitch/create-payment-vnpay", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var apiResponse = await JsonSerializer.DeserializeAsync<dynamic>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    string? paymentUrl = apiResponse?.GetProperty("paymentUrl").GetString();

                    if (!string.IsNullOrEmpty(paymentUrl))
                    {
                        return Redirect(paymentUrl); // Chuyển hướng người dùng đến cổng VNPay
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Không thể lấy URL thanh toán từ API.";
                        return RedirectToAction("PaymentFailed", "Payment");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Lỗi khi khởi tạo thanh toán: {response.StatusCode} - {errorContent}";
                    return RedirectToAction("PaymentFailed", "Payment");
                }
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = $"Lỗi kết nối API: {ex.Message}";
                return RedirectToAction("PaymentFailed", "Payment");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi không mong muốn: {ex.Message}";
                return RedirectToAction("PaymentFailed", "Payment");
            }
        }

        // Action cho trang thanh toán thành công
        [HttpGet]
        public IActionResult PaymentSuccess()
        {
            ViewBag.Message = TempData["SuccessMessage"] ?? "Thanh toán thành công!";
            return View();
        }

        // Action cho trang thanh toán thất bại
        [HttpGet]
        public IActionResult PaymentFailed(string? message)
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"] ?? message ?? "Thanh toán thất bại. Vui lòng thử lại.";
            return View();
        }

    }
}

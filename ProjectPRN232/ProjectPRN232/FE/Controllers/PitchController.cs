using System.Security.Claims;
using System.Text;
using System.Text.Json;
using FE.Models;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class PitchController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7089/api";

        public PitchController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/pitch");
            List<PitchViewModel> pitches = new List<PitchViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                pitches = await JsonSerializer.DeserializeAsync<List<PitchViewModel>>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<PitchViewModel>();
            }
            else
            {
                ViewBag.ErrorMessage = "Không thể tải dữ liệu từ API.";
            }
            return View(pitches);
        }

        public async Task<IActionResult> Detail(string id, string? selectedDate)
        {
            var client = _httpClientFactory.CreateClient();
            var viewModel = new PitchDetailViewModel();

            // 1. Lấy thông tin chi tiết sân bóng
            var pitchResponse = await client.GetAsync($"{_apiBaseUrl}/pitch/{id}");
            if (pitchResponse.IsSuccessStatusCode)
            {
                var pitchStream = await pitchResponse.Content.ReadAsStreamAsync();
                viewModel.Pitch = await JsonSerializer.DeserializeAsync<PitchViewModel>(pitchStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new PitchViewModel();
            }
            else
            {
                return NotFound($"Không tìm thấy sân bóng có ID = {id}.");
            }

            // 2. Lấy danh sách feedback
            var feedbackResponse = await client.GetAsync($"{_apiBaseUrl}/feedbackpitch/byPitch/{id}");
            if (feedbackResponse.IsSuccessStatusCode)
            {
                var feedbackStream = await feedbackResponse.Content.ReadAsStreamAsync();
                // Sửa lại: Dùng JsonResponseWrapper<T> để đọc cấu trúc {"$values": [...]}
                // Sửa lại: Đọc trực tiếp thành List vì API trả về mảng JSON đơn giản
                viewModel.Feedbacks = await JsonSerializer.DeserializeAsync<List<FeedbackViewModel>>(feedbackStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<FeedbackViewModel>();
            }
                
            // 3. Xử lý ngày
            if (DateOnly.TryParse(selectedDate, out DateOnly date))
            {
                viewModel.SelectedDate = date;
            }
            else
            {
                viewModel.SelectedDate = DateOnly.FromDateTime(DateTime.Now);
            }

            // 4. Lấy các khung giờ còn trống
            var slotsResponse = await client.GetAsync($"{_apiBaseUrl}/pitch/{id}/available-slots?date={viewModel.SelectedDate:yyyy-MM-dd}");
            if (slotsResponse.IsSuccessStatusCode)
            {
                var slotsStream = await slotsResponse.Content.ReadAsStreamAsync();
                // Sửa lại: Áp dụng logic tương tự cho khung giờ trống
                // Đọc trực tiếp thành List
                viewModel.AvailableSlots = await JsonSerializer.DeserializeAsync<List<BookingTimeViewModel>>(slotsStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<BookingTimeViewModel>();
            }

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddFeedback(FeedbackCreateViewModel newFeedback)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Chuẩn bị đối tượng DTO để gửi đến API
            var feedbackDto = new
            {
                UserId = userId, // UserId được set cứng là 1 theo yêu cầu
                PitchId = newFeedback.PitchId,
                Content = newFeedback.Content,
                Rating = newFeedback.Rating
            };

            var client = _httpClientFactory.CreateClient();
            // Chuyển đổi đối tượng C# thành chuỗi JSON
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(feedbackDto),
                Encoding.UTF8,
                "application/json"
            );

            // Gửi yêu cầu POST đến API
            var response = await client.PostAsync($"{_apiBaseUrl}/feedbackpitch", jsonContent);

            // Xử lý kết quả trả về từ API
            if (!response.IsSuccessStatusCode)
            {
                // Nếu có lỗi, lưu thông báo để hiển thị cho người dùng
                TempData["ErrorMessage"] = "Gửi phản hồi thất bại. Vui lòng thử lại.";
            }
            else
            {
                TempData["SuccessMessage"] = "Cảm ơn bạn đã gửi phản hồi!";
            }

            // Tải lại trang chi tiết để hiển thị feedback mới
            return RedirectToAction("Detail", new { id = newFeedback.PitchId });
        }
    }
}
using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service.IServices;
using Service.Services;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectPRN232.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TotalInvoicePitchController : ControllerBase
    {
        private readonly ITotalInvoicePitchService _totalInvoiceService;
        private readonly IInvoicePitchService _invoicePitchService; // Cần để tạo các InvoicePitchItem
        private readonly IVnPayService _vnpayService;

        public TotalInvoicePitchController(
                    ITotalInvoicePitchService totalInvoiceService,
                    IInvoicePitchService invoicePitchService, // Thêm vào constructor
                    IVnPayService vnpayService // Thêm vào constructor
                                               // IOptions<VnPayConfig> vnpayConfig // Thêm vào constructor
                    )
        {
            _totalInvoiceService = totalInvoiceService;
            _invoicePitchService = invoicePitchService;
            _vnpayService = vnpayService;
        }

        // GET: api/TotalInvoicePitch
        [HttpGet]
        [EnableQuery]
        public IActionResult GetTotalInvoices()
        {
            return Ok(_totalInvoiceService.GetTotalInvoicesAsQueryable());
        }

        // GET: api/TotalInvoicePitch/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTotalInvoice(int id)
        {
            var totalInvoiceWithDetails = await _totalInvoiceService.GetTotalInvoiceByIdAsync(id);
            if (totalInvoiceWithDetails == null)
            {
                return NotFound();
            }
            return Ok(totalInvoiceWithDetails);
        }

        // POST: api/TotalInvoicePitch
        [HttpPost]
        public async Task<IActionResult> CreateTotalInvoice(TotalInvoicePitchCreateDTO createDto)
        {
            var createdInvoice = await _totalInvoiceService.CreateTotalInvoiceAsync(createDto);
            return CreatedAtAction(nameof(GetTotalInvoice), new { id = createdInvoice.TotalInvoiceId }, createdInvoice);
        }

        // PUT: api/TotalInvoicePitch/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTotalInvoice(int id, TotalInvoicePitchUpdateDTO updateDto)
        {
            var updatedInvoice = await _totalInvoiceService.UpdateTotalInvoiceAsync(id, updateDto);
            if (updatedInvoice == null)
            {
                return NotFound("Không tìm thấy hóa đơn để cập nhật.");
            }
            return NoContent();
        }

        // DELETE: api/TotalInvoicePitch/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTotalInvoice(int id)
        {
            var success = await _totalInvoiceService.DeleteTotalInvoiceAsync(id);
            if (!success)
            {
                return NotFound("Không tìm thấy hóa đơn để xóa.");
            }
            return NoContent();
        }
        [HttpGet("Revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var totalRevenue = await _totalInvoiceService.GetTotalRevenueAsync();
            return Ok(new { TotalRevenue = totalRevenue });
        }

        [HttpGet("Revenue/Breakdown")]
        public async Task<IActionResult> GetRevenueBreakdown()
        {
            var revenueData = await _totalInvoiceService.GetRevenueByYearAndMonthAsync();
            return Ok(revenueData);
        }

        [HttpGet("withdetails")]
        public async Task<IActionResult> GetAllInvoicesWithDetails()
        {
            var invoices = await _totalInvoiceService.GetAllInvoicesWithDetailsAsync();
            return Ok(invoices);
        }

        [HttpGet("byuser/{userId}")]
        public async Task<IActionResult> GetInvoicesByUser(int userId)
        {
            var invoices = await _totalInvoiceService.GetInvoicesByUserIdAsync(userId);
            if (invoices == null || !invoices.Any())
            {
                return NotFound("Không tìm thấy hóa đơn nào cho người dùng này.");
            }
            return Ok(invoices);
        }
        [HttpPost("create-payment-vnpay")]
        [Authorize]
        public async Task<IActionResult> CreateVnPayPayment([FromBody] PaymentRequestDTO paymentRequest)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Người dùng chưa đăng nhập hoặc không hợp lệ.");
            }
            paymentRequest.UserId = userId;

            // 1. Tạo TotalInvoicePitch
            var createTotalInvoiceDto = new TotalInvoicePitchCreateDTO
            {
                UserId = paymentRequest.UserId,
                BookTime = paymentRequest.SelectedDate,
            };

            // Sửa: Gọi CreateTotalInvoiceAsync mà không truyền totalPrice
            var createdTotalInvoice = await _totalInvoiceService.CreateTotalInvoiceAsync(createTotalInvoiceDto);

            // 2. Tạo các InvoicePitch (chi tiết hóa đơn)
            foreach (var slotId in paymentRequest.SelectedSlots)
            {
                var createInvoicePitchItemDto = new InvoicePitchCreateDTO
                {
                    PitchId = paymentRequest.PitchId,
                    TotalInvoiceId = createdTotalInvoice.TotalInvoiceId,
                    BookingTimeId = slotId
                };
                await _invoicePitchService.CreateInvoicePitchAsync(createInvoicePitchItemDto);
            }

            // Gán OrderId là TotalInvoiceId đã tạo
            paymentRequest.OrderId = createdTotalInvoice.TotalInvoiceId.ToString();
            paymentRequest.OrderDescription = $"Thanh toan don hang {paymentRequest.OrderId} san {paymentRequest.PitchId} ngay {paymentRequest.SelectedDate}";

            // 3. Tạo URL thanh toán VNPay
            var paymentUrl = _vnpayService.CreatePaymentUrl(paymentRequest, HttpContext);

            if (string.IsNullOrEmpty(paymentUrl))
            {
                return BadRequest("Không thể tạo URL thanh toán VNPay. Vui lòng thử lại.");
            }

            return Ok(new { paymentUrl });
        }

        // Endpoint để VNPay callback về sau khi thanh toán
        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            // Định nghĩa URL của Frontend MVC của bạn
            // THAY THẾ "https://localhost:7223" BẰNG URL CHÍNH XÁC CỦA DỰ ÁN FRONTEND CỦA BẠN
            string frontendBaseUrl = "https://localhost:7255"; // Ví dụ: Cổng của dự án FE

            var response = _vnpayService.VnPayReturn(Request.Query);

            if (int.TryParse(response.OrderId, out int totalInvoiceId))
            {
                var totalInvoiceWithDetails = await _totalInvoiceService.GetTotalInvoiceByIdAsync(totalInvoiceId);
                if (totalInvoiceWithDetails == null)
                {
                    // Chuyển hướng về trang Failed với thông báo lỗi
                    return Redirect($"{frontendBaseUrl}/Payment/PaymentFailed?message={WebUtility.UrlEncode("Mã hóa đơn không hợp lệ.")}");
                }

                var calculatedTotalPrice = totalInvoiceWithDetails.InvoicePitches.Sum(ip => ip.Price ?? 0);

                // THAY ĐỔI: So sánh số tiền nhận từ VNPay (response.Amount) với số tiền tính toán lại
                // Chú ý: response.Amount là long, calculatedTotalPrice là int. Cần ép kiểu hoặc đảm bảo khớp.
                // Nếu Price là int trong InvoicePitch, thì Sum sẽ là int.
                if (calculatedTotalPrice != response.Amount)
                {
                    return Redirect($"{frontendBaseUrl}/Payment/PaymentFailed?message={WebUtility.UrlEncode("Số tiền thanh toán không khớp với đơn hàng. Vui lòng liên hệ hỗ trợ.")}");
                }

                if (response.Success)
                {
                    // Chuyển hướng về trang thành công của frontend
                    return Redirect($"{frontendBaseUrl}/Payment/PaymentSuccess?message={WebUtility.UrlEncode("Thanh toán thành công!")}");
                }
                else
                {
                    // Chuyển hướng về trang thất bại của frontend
                    return Redirect($"{frontendBaseUrl}/Payment/PaymentFailed?message={WebUtility.UrlEncode(response.Message)}");
                }
            }
            else
            {
                return Redirect($"{frontendBaseUrl}/Payment/PaymentFailed?message={WebUtility.UrlEncode("Mã hóa đơn từ VNPay không hợp lệ.")}");
            }
        }
    }
}
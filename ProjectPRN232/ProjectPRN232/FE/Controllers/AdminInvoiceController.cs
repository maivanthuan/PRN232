using FE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FE.Controllers
{
    public class AdminInvoiceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7089/api";

        public AdminInvoiceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_apiBaseUrl}/TotalInvoicePitch/withdetails");

            List<AdminInvoiceViewModel> invoices = new List<AdminInvoiceViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                // SỬA LỖI Ở ĐÂY: Vì API giờ đã trả về một mảng JSON sạch,
                // chúng ta có thể deserialize trực tiếp vào List<T>.
                invoices = await JsonSerializer.DeserializeAsync<List<AdminInvoiceViewModel>>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AdminInvoiceViewModel>();
            }
            else
            {
                ViewBag.ErrorMessage = "Không thể tải danh sách hóa đơn từ API.";
            }

            return View(invoices);
        }
    }
}

using System.Text.Json.Serialization;

namespace FE.Models
{
    public class AdminInvoiceViewModel
    {
        [JsonPropertyName("totalInvoiceId")]
        public int TotalInvoiceId { get; set; }

        [JsonPropertyName("userId")]
        public int? UserId { get; set; }

        // Giả sử API trả về UserName, nếu không bạn cần join trong BE
        [JsonPropertyName("userName")]
        public string? UserName { get; set; }

        [JsonPropertyName("Name")] // Đảm bảo khớp với tên thuộc tính trong DTO của API
        public string? Name { get; set; }


        [JsonPropertyName("bookTime")]
        public DateOnly? BookTime { get; set; }

        [JsonPropertyName("totalPrice")]
        public int? TotalPrice { get; set; }

        [JsonPropertyName("invoicePitches")]
        public List<InvoicePitchItemViewModel> InvoicePitches { get; set; } = new List<InvoicePitchItemViewModel>();
    }
}

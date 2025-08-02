using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class InvoicePitchDTO
    {
        [Key] // Cần thiết cho OData nếu bạn dùng [EnableQuery]
        public int InvoicePitchId { get; set; }
        public string? PitchId { get; set; }
        public int? PricePitchId { get; set; }
        public int? TotalInvoiceId { get; set; }
        public int? BookingTimeId { get; set; }

        // Thuộc tính lấy từ bảng PricePitch
        public int? Price { get; set; }

        // Thuộc tính lấy từ bảng BookingTime
        public string? Time { get; set; }
    }
    public class InvoicePitchCreateDTO
    {
        public string? PitchId { get; set; }
        public int? TotalInvoiceId { get; set; }
        public int? BookingTimeId { get; set; }
    }
    public class InvoicePitchUpdateDTO
    {
        public string? PitchId { get; set; }
        public int? PricePitchId { get; set; }
        public int? TotalInvoiceId { get; set; }
        public int? BookingTimeId { get; set; }
    }
}

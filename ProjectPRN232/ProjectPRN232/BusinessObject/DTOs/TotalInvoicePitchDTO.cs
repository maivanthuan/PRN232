using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class TotalInvoicePitchDTO
    {
        [Key]
        public int TotalInvoiceId { get; set; }
        public int? UserId { get; set; }
        public DateOnly? BookTime { get; set; }
        public int? TotalPrice { get; set; }
    }

    public class TotalInvoicePitchCreateDTO
    {
        public int? UserId { get; set; }
        public DateOnly? BookTime { get; set; }
    }

    public class TotalInvoicePitchUpdateDTO
    {
        public int? UserId { get; set; }
        public DateOnly? BookTime { get; set; }
    }
    public class TotalInvoiceWithDetailsDTO
    {
        public int TotalInvoiceId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public DateOnly? BookTime { get; set; }
        public int? TotalPrice { get; set; }
        public List<InvoicePitchItemDTO> InvoicePitches { get; set; } = new List<InvoicePitchItemDTO>();
    }
}

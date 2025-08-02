using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class InvoicePitchItemDTO
    {
        public int InvoicePitchId { get; set; }
        public string? PitchId { get; set; }
        public int? PricePitchId { get; set; }
        public int? BookingTimeId { get; set; }
        public int? Price { get; set; }
        public string? Time { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace FE.Models
{
    public class BookingTimeViewModel
    {

        [JsonPropertyName("bookingTimeId")]
        public int BookingTimeId { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }

    }
}

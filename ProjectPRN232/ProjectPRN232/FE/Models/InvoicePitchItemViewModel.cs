using System.Text.Json.Serialization;

namespace FE.Models
{
    public class InvoicePitchItemViewModel
    {
        [JsonPropertyName("invoicePitchId")]
        public int InvoicePitchId { get; set; }

        [JsonPropertyName("pitchId")]
        public string? PitchId { get; set; }

        [JsonPropertyName("price")]
        public int? Price { get; set; }

        [JsonPropertyName("time")]
        public string? Time { get; set; }
    }
}

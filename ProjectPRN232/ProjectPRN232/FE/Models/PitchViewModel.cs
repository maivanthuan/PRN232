using System.Text.Json.Serialization;

namespace FE.Models
{
    public class PitchViewModel
    {
        [JsonPropertyName("pitchId")]
        public string? PitchId { get; set; }

        [JsonPropertyName("pitchType")]
        public int PitchType { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("pricePitch")]
        public int PricePitch { get; set; }
    }

    public class ODataResponse<T>
    {
        [JsonPropertyName("value")]
        // Sửa: Khởi tạo giá trị mặc định để tránh null
        public List<T> Value { get; set; } = new List<T>();
    }
}

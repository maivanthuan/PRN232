using System.Text.Json.Serialization;

namespace FE.Models
{
    public class FeedbackViewModel
    {
        [JsonPropertyName("feedbackId")]
        public int FeedbackId { get; set; }

        [JsonPropertyName("userId")]
        public int? UserId { get; set; }

        [JsonPropertyName("pitchId")]
        public string? PitchId { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("rating")]
        public double? Rating { get; set; }

        [JsonPropertyName("timeFeedback")]
        public DateOnly? TimeFeedback { get; set; }

        [JsonPropertyName("userName")]
        public string? UserName { get; set; }
    }
}

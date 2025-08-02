using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class FeedbackPitchDto
    {
        public int FeedbackId { get; set; }
        public int? UserId { get; set; }
        public string? PitchId { get; set; }
        public string? Content { get; set; }
        public double? Rating { get; set; }
        public DateOnly? TimeFeedback { get; set; }
        public string? UserName { get; set; } // To display user    
    }

    public class FeedbackPitchCreateDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string PitchId { get; set; } = null!;
        public string? Content { get; set; }
        [Range(0, 5)]
        public double? Rating { get; set; }
    }

    public class FeedbackPitchUpdateDto
    {
        public string? Content { get; set; }
        [Range(0, 5)]
        public double? Rating { get; set; }
    }
}

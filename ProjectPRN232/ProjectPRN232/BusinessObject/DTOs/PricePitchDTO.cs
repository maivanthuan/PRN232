// BusinessObject/DTOs/PricePitchDTO.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTOs
{
    public class PricePitchDTO
    {
        public int PricePitchId { get; set; }
        public string? PitchId { get; set; }
        public int? Price { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
    }
}
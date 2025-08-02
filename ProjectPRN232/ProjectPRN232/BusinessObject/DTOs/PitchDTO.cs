using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BusinessObject.DTOs


{
    public class PitchDTO {
        [Key]
        public string PitchId { get; set; } = null!;

        public int PitchType { get; set; }

        public string? Image { get; set; }

        public int Status { get; set; }

        public int PricePitch { get; set; }
    }
    public class PitchCreateDTO
    {

        [Key]
        public string PitchId { get; set; } = null!;
        public int PitchType { get; set; }
        public string? Image { get; set; }
        public int Status { get; set; }
        public IFormFile? ImageFile { get; set; } // New property for file upload
        // Giá sân
        public int Price { get; set; }

        // Chỉ cần nhập ngày kết thúc
        [Required]
        public DateTime TimeEnd { get; set; }
    }
    public class PitchUpdateDTO 
    {
        public string PitchId { get; set; } = null!;

        public int PitchType { get; set; }
        public string? Image { get; set; }
        public int Status { get; set; }
        public IFormFile? ImageFile { get; set; } // New property for file upload

        // Thông tin để tạo bản ghi giá mới
        [Required]
        public int Price { get; set; }

        [Required]
        public DateTime TimeEnd { get; set; }
    }
}

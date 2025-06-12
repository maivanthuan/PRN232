using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.DTO
{
    public class CategoryDto
    {
        
        public short CategoryId { get; set; }

            [Required]
            [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public bool CategoryStatus { get; set; }
      
        [Required]
        [StringLength(200)]
        public string CategoryDesciption { get; set; } = string.Empty;

        public short? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
}

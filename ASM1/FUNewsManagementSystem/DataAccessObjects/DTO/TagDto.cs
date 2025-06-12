using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.DTO
{
    public class TagDto
    {
        public int TagId { get; set; }

        [Required]
        [StringLength(50)]
        public string TagName { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}

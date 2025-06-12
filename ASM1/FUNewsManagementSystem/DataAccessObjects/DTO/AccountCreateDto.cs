using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.DTO
{
    public class AccountCreateDto
    {
        [Required]
        [StringLength(100)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string AccountEmail { get; set; } = string.Empty;

        [Range(1, 2)]
        public int AccountRole { get; set; }

        [Required]
        [MinLength(6)]
        public string AccountPassword { get; set; } = string.Empty;
    }

}

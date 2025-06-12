using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.DTO
{
    public class AccountDto
    {
        public short AccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string AccountEmail { get; set; } = string.Empty;

        public int AccountRole { get; set; }
    }

}

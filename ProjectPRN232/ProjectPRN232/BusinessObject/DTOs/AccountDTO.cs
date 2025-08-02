using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;
        public string Name { get; set; }
        public byte? StatusOtp { get; set; }
        public string Role { get; set; }
    }
    public class ChangePasswordDTO
    {
        public string Password { get; set; }
        public string CurrentPassword { get; set; } = null!;
    }
    public class AccountDTO
    {
        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Name { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public int? RoleId { get; set; }
        public byte? StatusOtp { get; set; }

        public string? Avata { get; set; }

        public string? DateOfBirth { get; set; }
    }

    public class AccountCreateDTO
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Name { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string Password { get; set; } = null!;

        public int? RoleId { get; set; }

        public string? Avata { get; set; }

        public string? DateOfBirth { get; set; }
    }

    public class AccountUpdateDTO
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public int? RoleId { get; set; }

        public string? Avata { get; set; }

        public string? DateOfBirth { get; set; }
    }
    public class BlockUserDTO
    {
        public int UserId { get; set; }
        public byte? StatusOtp { get; set; }  // Sẽ toggle 0/1
    }
}

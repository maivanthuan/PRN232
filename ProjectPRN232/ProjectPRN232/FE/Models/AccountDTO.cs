using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Name { get; set; }
        public string Role { get; set; }
    }
    public class ChangePasswordDTO
    {


        public string? CurrentPassword { get; set; }

        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu mới và mật khẩu xác nhận không khớp.")]
        public string? ConfirmNewPassword { get; set; }
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

        public string? Avata { get; set; }

        public string? DateOfBirth { get; set; }
    }

    public class AccountCreateDTO // <-- DTO cho việc tạo tài khoản mới
    {
        [Required(ErrorMessage = "Tên tài khoản là bắt buộc.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên tài khoản phải dài từ 3 đến 50 ký tự.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        public string Email { get; set; } = null!;

        [StringLength(100)] // Thêm ErrorLength nếu muốn
        public string? Name { get; set; }

        public string? Gender { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải dài từ 6 đến 100 ký tự.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        // THÊM TRƯỜNG XÁC NHẬN MẬT KHẨU NÀY CHO VALIDATION Ở FE
        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; } = null!;

        // Bỏ RoleId ở đây hoặc để nguyên nếu BE không cần nó từ FE
        // public int? RoleId { get; set; } // Nếu đã gán ở BE, FE không cần gửi
        public string? Avata { get; set; }
        public string? DateOfBirth { get; set; }
    }

    public class AccountUpdateDTO
    {

        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
    }
}


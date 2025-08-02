using System.ComponentModel.DataAnnotations;

namespace FE.Models
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = null!;

        [Display(Name = "Ghi nhớ tôi")] // Thuộc tính bổ sung cho FE
        public bool RememberMe { get; set; }
    }
}

namespace FE.Models
{
    public class AccountViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public int? RoleId { get; set; }
        public string? Avata { get; set; }
        public string? DateOfBirth { get; set; }
        public byte? StatusOtp { get; set; }  // Để hiển thị và toggle
    }
}
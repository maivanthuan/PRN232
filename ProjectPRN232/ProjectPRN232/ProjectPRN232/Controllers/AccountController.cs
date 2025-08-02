using System.Security.Claims;
using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service.IServices;

namespace ProjectPRN232.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/accounts
        [HttpGet]
        [EnableQuery]
        public IActionResult GetAccounts()
        {
            return Ok(_accountService.GetAccountsAsQueryable());
        }

        // GET: api/accounts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        // POST: api/accounts
        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountCreateDTO accountCreateDTO)
        {
            var createdAccount = await _accountService.CreateAccountAsync(accountCreateDTO);
            accountCreateDTO.RoleId = 2;
            return CreatedAtAction(nameof(GetAccount), new { id = createdAccount.UserId }, createdAccount);
        }

        // PUT: api/accounts/5
        [HttpPut("{id}")]
        [Authorize]// Yêu cầu phải đăng nhập để cập nhật
        public async Task<IActionResult> UpdateAccount(int id, AccountUpdateDTO accountUpdateDTO)
        {
            // 1. Lấy thông tin tài khoản gốc từ DB để kiểm tra
            var accountFromDb = await _accountService.GetAccountByIdAsync(id);

            // 2. Nếu không tìm thấy tài khoản, trả về NotFound
            if (accountFromDb == null)
            {
                return NotFound();
            }

            // 4. Thực hiện cập nhật
            var updatedAccount = await _accountService.UpdateAccountAsync(id, accountUpdateDTO);
            if (updatedAccount == null)
            {
                return BadRequest("Không thể cập nhật tài khoản.");
            }
            return NoContent();
        }

        // DELETE: api/accounts/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var success = await _accountService.DeleteAccountAsync(id);
            if (!success)
            {
                return Forbid();
            }
            return NoContent();
        }


        [HttpPut("change-password")]
        [Authorize] // Đảm bảo người dùng phải được xác thực để gọi API này
        public async Task<IActionResult> ChangePassword([FromBody] BusinessObject.DTOs.ChangePasswordDTO changePasswordDto) // Sử dụng DTO từ BusinessObject
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Không tìm thấy ID người dùng trong token hoặc ID không hợp lệ." });
            }

            // Kiểm tra validation từ Model State (ví dụ: required, minlength từ DTO của BusinessObject)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về lỗi validation tự động
            }

            try
            {
                // Gọi service để đổi mật khẩu
                var success = await _accountService.ChangePasswordAsync(userId, changePasswordDto);

                if (success)
                {
                    return Ok(new { message = "Đổi mật khẩu thành công." });
                }
                // Trường hợp này có thể xảy ra nếu GetByIdAsync trong service trả về null
                // hoặc SaveChangesAsync không thành công vì lý do nào đó không phải do CurrentPassword sai.
                return StatusCode(500, new { message = "Không thể cập nhật mật khẩu. Vui lòng thử lại." });
            }
            catch (ArgumentException ex) // Bắt lỗi nếu mật khẩu hiện tại không đúng
            {
                // Trả về BadRequest với thông báo lỗi cụ thể
                ModelState.AddModelError("CurrentPassword", ex.Message);
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về lỗi server
                return StatusCode(500, new { message = $"Đã xảy ra lỗi không mong muốn: {ex.Message}" });
            }
        }

        [HttpPut("toggle-block/{id}")]
        //[Authorize(Roles = "Admin")]  // Sau này thêm để chỉ admin dùng
        public async Task<IActionResult> ToggleBlock(int id)
        {
            var success = await _accountService.ToggleBlockAsync(id);
            if (!success) return NotFound("Không tìm thấy tài khoản hoặc không thể toggle.");
            return NoContent();
        }
    }
}

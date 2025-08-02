using BusinessObject.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;

namespace ProjectPRN232.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AuthController(IAccountService userService)
        {
            _accountService = userService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Username) || string.IsNullOrEmpty(loginDTO.Password))
            {
                return BadRequest("Invalid login request.");
            }
            UserDTO? userDTO;
            var token = _accountService.Authenticate(loginDTO, out userDTO);
            if (token == null || userDTO == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(new
            {
                Token = token,
                User = userDTO
            });
        }
    }
}

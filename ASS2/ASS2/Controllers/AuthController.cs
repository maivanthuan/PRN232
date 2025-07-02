using BusinessObject.DTO;
using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OData.UriParser;
using Service.IService;
using Service.Service;

namespace ASS2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        public readonly IOptions<AdminConfig> _adminConfig;
        public readonly ITokenService _tokenService;
        public readonly FunewsManagementContext _context;
        public AuthController(IOptions<AdminConfig> adminConfig, ITokenService tokenService, FunewsManagementContext context)
        {
            _adminConfig = adminConfig;
            _tokenService = tokenService;
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login( LoginDTO request)
        {
            if (request.Email == _adminConfig.Value.Email &&
                request.Password == _adminConfig.Value.Password)
            {
                var token = _tokenService.GenerateToken(request.Email, "Admin");
                return Ok(new { token });
            }
            var user = _context.SystemAccounts.FirstOrDefault(u => u.AccountEmail == request.Email && u.AccountPassword == request.Password);
            if (user != null)
            {
                string role = user.AccountRole switch
                {
                    1 => "Staff",
                    2 => "Lecturer",
                    _ => "Unknown"
                };
                if(role == "Unknown")
                {
                    return Unauthorized("Invalid role");
                }
                var token = _tokenService.GenerateToken(request.Email, role);
                return Ok(new { token });
            }
            return Unauthorized("Email sai hoặc sai mật khẩu");
        }

    }
}

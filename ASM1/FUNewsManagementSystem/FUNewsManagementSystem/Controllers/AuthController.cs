using AutoMapper;
using DataAccessObjects.DTO;
using FUNewsManagementSystem.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repositories.IRepository;

namespace FUNewsManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ISystemAccountRepository _repo;
        private readonly IMapper _mapper;
        private readonly AdminConfig _adminConfig;

        public AuthController(ISystemAccountRepository repo, IMapper mapper, IOptions<AdminConfig> adminConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _adminConfig = adminConfig.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginDto dto)
        {
            if (dto.Email == _adminConfig.Email && dto.Password == _adminConfig.Password)
            {
                return Ok(new { Role = "Admin", Name = "Administrator" });
            }

            var acc = await _repo.GetByEmailAsync(dto.Email);
            if (acc == null || acc.AccountPassword != dto.Password)
                return Unauthorized();

            return Ok(new
            {
                Role = acc.AccountRole == 1 ? "Staff" : "Lecturer",
                Name = acc.AccountName,
                Id = acc.AccountId
            });
        }
    }
}

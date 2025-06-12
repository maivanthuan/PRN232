using DataAccessObjects.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _service;

        public ProfileController(IProfileService service)
        {
            _service = service;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            var profile = await _service.GetProfileByEmailAsync(email);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> Update(string email, [FromBody] AccountCreateDto dto)
        {
            var result = await _service.UpdateProfileAsync(email, dto);
            if (!result) return NotFound();
            return Ok(new { message = "Profile updated successfully." });
        }
    }
}

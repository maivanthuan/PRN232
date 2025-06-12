using DataAccessObjects.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace FUNewsManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _service.GetAllAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var account = await _service.GetByIdAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateDto dto)
        {
            await _service.AddAsync(dto);
            return Ok(new { message = "Account created successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(short id, AccountCreateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { message = "Account updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return BadRequest("Cannot delete account with linked news.");
            return Ok(new { message = "Account deleted successfully." });
        }
    }
}

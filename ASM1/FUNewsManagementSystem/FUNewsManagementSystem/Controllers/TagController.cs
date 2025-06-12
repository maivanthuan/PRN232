using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace FUNewsManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _service;

        public TagController(ITagService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _service.GetAllAsync();
            return Ok(tags);
        }

        [HttpGet("unused")]
        public async Task<IActionResult> GetUnused()
        {
            var tags = await _service.GetUnusedAsync();
            return Ok(tags);
        }
    }
}

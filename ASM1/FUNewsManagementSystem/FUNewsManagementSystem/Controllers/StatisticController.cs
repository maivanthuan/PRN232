using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _service;

        public StatisticController(IStatisticService service)
        {
            _service = service;
        }

        [HttpGet("news")]
        public async Task<IActionResult> GetNewsByPeriod([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var result = await _service.GetNewsByPeriodAsync(start, end);
            return Ok(result);
        }
    }
}

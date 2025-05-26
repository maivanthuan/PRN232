using FestivalLab.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FestivalLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FestivalsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Festival> Get()
        {
            return new List<Festival> {
      new Festival { Id = 1, Name = "Festival Hue", Province = "Hue", Date = new DateTime(2024, 3, 22) },
      new Festival { Id = 2, Name = "Festival Da Lat", Province = "Lam Dam", Date = new DateTime(2024, 2, 10) }
    };
        }
        [HttpGet("json")]
        public JsonResult GetJson() => new JsonResult(new { Message = "Hello JSON" });

        [HttpGet("text")]
        public ContentResult GetText() => Content("Hello Plain Text", "text/plain");

        [HttpGet("auto")]
        public IActionResult GetAuto() => Ok(new { Message = "Auto format" });


    }

}

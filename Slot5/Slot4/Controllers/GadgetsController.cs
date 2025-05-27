using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Slot4.DBContext;

namespace Slot4.Controllers
{
    [Route("gadget")]
    [ApiController]
    public class GadgetsController : ControllerBase
    {
        private readonly MyWorldDbContext _worldContext;    
        public GadgetsController(MyWorldDbContext worldContext)
        {
            _worldContext = worldContext;
        }
        [EnableQuery]
        [HttpGet("Get")]
        public IActionResult Get()
        {
            return Ok(_worldContext.Gadgets.AsQueryable());
        }
    }
}

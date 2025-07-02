using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service.IService;
using Service.Service;

namespace ASS2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SystemAccountsController : ControllerBase
    {
        private readonly ISystemAccountService _service;
        public SystemAccountsController(ISystemAccountService service) => _service = service;

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var acc = _service.GetById(id);
            return acc == null ? NotFound() : Ok(acc);
        }
        [EnableQuery]
        [HttpGet("/odata/Systems")]
        public IQueryable<SystemAccount> GetAllOData() => _service.GetAll();

        [HttpPost]
        public IActionResult Create(SystemAccount acc)
        {
            _service.Add(acc);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(short id, SystemAccount acc)
        {
            acc.AccountId = id;
            _service.Update(acc);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }




    }
}
            
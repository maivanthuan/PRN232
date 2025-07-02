using Ass1.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Ass1.Controllers
{
    [Route("odata/SystemAccounts")]
    [ApiController]
    public class SystemAccountsController : ODataController
    {
        private readonly ISystemAccountRepository _repository;

        public SystemAccountsController(ISystemAccountRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ActionName("GetAll")]
        public IActionResult Get()
        {
            return Ok(_repository.GetAllAsync().Result);
        }

        [HttpGet("{id}")]
        [ActionName("GetById")]
        public IActionResult Get([FromRoute] short key)
        {
            var account = _repository.GetByIdAsync(key).Result;
            if (account == null) return NotFound();
            return Ok(account);
        }
    }
}

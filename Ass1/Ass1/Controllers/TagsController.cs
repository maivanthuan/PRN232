using Ass1.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Ass1.Controllers
{
    [Route("odata/Tags")]
    [ApiController]
    public class TagsController : ODataController
    {
        private readonly ITagRepository _repository;

        public TagsController(ITagRepository repository)
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
        public IActionResult Get([FromRoute] int key)
        {
            var tag = _repository.GetByIdAsync(key).Result;
            if (tag == null) return NotFound();
            return Ok(tag);
        }
    }
}

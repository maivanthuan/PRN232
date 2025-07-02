using Ass1.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Ass1.Controllers
{
    [Route("odata/Categories")]
    [ApiController]
    public class CategoriesController : ODataController
    {

        private readonly ICategoryRepository _repository;

        public CategoriesController(ICategoryRepository repository)
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
        public IActionResult Get([FromRoute] short id)
        {
            var category = _repository.GetByIdAsync(id).Result;
            if (category == null) return NotFound();
            return Ok(category);
        }
    }
}


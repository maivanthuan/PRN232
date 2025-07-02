using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service.IService;
using Service.Service;

namespace ASS2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoriesController(ICategoryService service) => _service = service;

        [Authorize]
        [EnableQuery]

        [HttpGet("/odata/Cates")]
        public IQueryable<Category> GetAllOData() => _service.GetAll();

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var c = _service.GetById(id);
            return c == null ? NotFound() : Ok(c);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public IActionResult Create(Category c)
        {
            _service.Add(c);
            return Ok();
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}")]
        public IActionResult Update(short id, Category c)
        {
            c.CategoryId = id;
            _service.Update(c);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }

    }
}

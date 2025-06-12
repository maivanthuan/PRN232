using DataAccessObjects.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace FUNewsManagementSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _service.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto dto)
        {
            await _service.AddAsync(dto);
            return Ok(new { message = "Category created successfully." });
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDto dto)
        {
            await _service.UpdateAsync(dto);
            return Ok(new { message = "Category updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return BadRequest("Cannot delete category linked with news.");
            return Ok(new { message = "Category deleted successfully." });
        }
    }
}

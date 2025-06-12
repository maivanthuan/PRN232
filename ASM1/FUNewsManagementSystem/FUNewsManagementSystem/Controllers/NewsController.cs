using DataAccessObjects.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace FUNewsManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _service;

        public NewsController(INewsService service)
        {
            _service = service;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveNews()
        {
            var news = await _service.GetActiveNewsAsync();
            return Ok(news);
        }

        [HttpGet("author/{authorId}")]
        public async Task<IActionResult> GetByAuthor(short authorId)
        {
            var news = await _service.GetByAuthorAsync(authorId);
            return Ok(news);
        }
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var news = await _service.GetByIdAsync(id);
            return Ok(news);
        }
        [HttpPost("{authorId}")]
        public async Task<IActionResult> Create(short authorId, NewsCreateDto dto)
        {
            await _service.CreateNewsAsync(dto, authorId);
            return Ok(new { message = "News article created successfully." });
        }
		
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(String id, [FromQuery] short authorId, [FromBody] NewsCreateDto dto)
		{
			var success = await _service.UpdateNewsAsync(id, dto, authorId);
			if (!success)
				return BadRequest("Không thể cập nhật. Bạn không phải là người tạo hoặc bài viết không tồn tại.");

			return Ok(new { message = "Cập nhật bài viết thành công." });
		}

		
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(String id, [FromQuery] short requesterId)
		{
			var success = await _service.DeleteNewsAsync(id, requesterId);
			if (!success)
				return BadRequest("Không thể xoá. Bạn không phải là người tạo hoặc bài viết không tồn tại.");

			return Ok(new { message = "Xoá bài viết thành công." });
		}
	}
}

using System.Security.Claims;
using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service.IServices;

namespace ProjectPRN232.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PitchController : ControllerBase
    {
        private readonly IPitchServices _pitchService;
        private readonly IBookingTimeService _bookingTimeService;

        public PitchController(IPitchServices pitchService, IBookingTimeService bookingTimeService)
        {
            _pitchService = pitchService;
            _bookingTimeService = bookingTimeService;
        }

        // GET: api/articles
        [HttpGet]
        [EnableQuery]
        public IActionResult GetPitch()
        {
            return Ok(_pitchService.GetPitchAsQueryable());
        }

        // GET: api/articles/5 (Đáp ứng yêu cầu Get details)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPitch(string id)
        {
            var article = await _pitchService.GetPitchByIdAsync(id);
            if (article == null) return NotFound();
            return Ok(article);
        }

        // POST: api/articles (Đáp ứng yêu cầu Create)
        [HttpPost]
        public async Task<IActionResult> CreatePitch(PitchCreateDTO pitchCreateDTO)
        {
           
            var createdPitch = await _pitchService.CreatePitchAsync(pitchCreateDTO);
            return CreatedAtAction(nameof(GetPitch), new { id = createdPitch.PitchId }, createdPitch);
        }

        // PUT: api/courses/5 (Đáp ứng yêu cầu Update)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePitch(string id, PitchUpdateDTO pitchUpdateDto)
        {
            // 1. Lấy thông tin khóa học gốc từ DB để kiểm tra
            var pitchFromDb = await _pitchService.GetPitchByIdAsync(id);

            // 2. Nếu không tìm thấy khóa học, trả về NotFound
            if (pitchFromDb == null)
            {
                return NotFound();
            }


            // 4. Thực hiện cập nhật
            var updatedPitch = await _pitchService.UpdatePitchAsync(id, pitchUpdateDto);
            if (updatedPitch == null)
            {
                return BadRequest("Không thể cập nhật sân bóng.");
            }
            return NoContent();
        }
        // DELETE: api/articles/5 (Đáp ứng yêu cầu Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePitch(string id)
        {
            var success = await _pitchService.DeletePitchAsync(id);
            if (!success)
            {
                return Forbid();
            }
            return NoContent();
        }
        [HttpGet("{pitchId}/available-slots")]
        public async Task<IActionResult> GetAvailableSlots(string pitchId, [FromQuery] DateOnly date)
        {
            if (date == default)
            {
                return BadRequest("Vui lòng cung cấp ngày hợp lệ.");
            }

            var availableSlots = await _bookingTimeService.GetAvailableSlotsAsync(pitchId, date);
            return Ok(availableSlots);
        }
    }
}

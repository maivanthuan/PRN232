using BusinessObject.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.IServices;

namespace ProjectPRN232.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackPitchController : ControllerBase
    {
        private readonly IFeedbackPitchService _feedbackPitchServices; 

        public FeedbackPitchController(IFeedbackPitchService feedbackPitchServices) 
        {
            _feedbackPitchServices = feedbackPitchServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           
            var feedbacks = await _feedbackPitchServices.GetFeedbackPitchesAsQueryable().ToListAsync();
            return Ok(feedbacks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var feedback = await _feedbackPitchServices.GetFeedbackPitchByIdAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return Ok(feedback);
        }

        [HttpGet("byPitch/{pitchId}")] 
        public async Task<IActionResult> GetByPitchId(string pitchId)
        {
            var feedbacks = await _feedbackPitchServices.GetFeedbackPitchByPitchIdAsync(pitchId);
            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound($"No feedback found for PitchId: {pitchId}");
            }
            return Ok(feedbacks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeedbackPitchCreateDto feedbackCreateDTO) 
        {
            var createdFeedback = await _feedbackPitchServices.CreateFeedbackPitchAsync(feedbackCreateDTO);
            return CreatedAtAction(nameof(GetById), new { id = createdFeedback.FeedbackId }, createdFeedback);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, FeedbackPitchUpdateDto feedbackUpdateDTO) 
        {
            var updatedFeedback = await _feedbackPitchServices.UpdateFeedbackPitchAsync(id, feedbackUpdateDTO);
            if (updatedFeedback == null)
            {
                return NotFound(); 
            }
            return NoContent(); 
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _feedbackPitchServices.DeleteFeedbackPitchAsync(id);
            if (!result)
            {
                return NotFound(); 
            }
            return NoContent(); 
        }
    }
}

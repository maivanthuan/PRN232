using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;

namespace Service.IServices
{
    public interface IFeedbackPitchService
    {
        Task<FeedbackPitchDto> CreateFeedbackPitchAsync(FeedbackPitchCreateDto feedbackPitchCreateDTO);
        Task<FeedbackPitchDto?> GetFeedbackPitchByIdAsync(int id);
        IQueryable<FeedbackPitchDto> GetFeedbackPitchesAsQueryable();
        Task<List<FeedbackPitchDto>> GetFeedbackPitchByPitchIdAsync(string pitchId);
        Task<FeedbackPitchDto?> UpdateFeedbackPitchAsync(int feedbackId, FeedbackPitchUpdateDto feedbackPitchUpdateDTO);
        Task<bool> DeleteFeedbackPitchAsync(int feedbackId);
    }
}

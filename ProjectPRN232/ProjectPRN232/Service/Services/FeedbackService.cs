using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.DTOs;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using Service.IServices;

namespace Service.Services
{
    public class FeedbackService : IFeedbackPitchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FeedbackPitchDto> CreateFeedbackPitchAsync(FeedbackPitchCreateDto feedbackPitchCreateDTO)
        {
            var feedbackPitch = _mapper.Map<FeedbackPitch>(feedbackPitchCreateDTO);

            await _unitOfWork.FeedbackPitch.AddAsync(feedbackPitch);
            await _unitOfWork.SaveChangesAsync();

            var createdFeedback = await GetFeedbackPitchByIdAsync(feedbackPitch.FeedbackId);
            return createdFeedback!;
        }

        public async Task<bool> DeleteFeedbackPitchAsync(int feedbackId)
        {
            var feedbackPitch = await _unitOfWork.FeedbackPitch.GetByIdAsync(feedbackId);
            if (feedbackPitch == null) return false;

            _unitOfWork.FeedbackPitch.Delete(feedbackPitch);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<FeedbackPitchDto?> GetFeedbackPitchByIdAsync(int id)
        {
            var feedbackPitch = await _unitOfWork.FeedbackPitch
                                                    .Get(fp => fp.FeedbackId == id, includeProperties: "User") 
                                                    .FirstOrDefaultAsync();
            return _mapper.Map<FeedbackPitchDto>(feedbackPitch);
        }

        public IQueryable<FeedbackPitchDto> GetFeedbackPitchesAsQueryable()
        {
            var feedbackPitches = _unitOfWork.FeedbackPitch.Get(includeProperties: "User"); 
            return feedbackPitches.ProjectTo<FeedbackPitchDto>(_mapper.ConfigurationProvider);
        }

        public async Task<List<FeedbackPitchDto>> GetFeedbackPitchByPitchIdAsync(string pitchId)
        {
            var feedbackPitches = await _unitOfWork.FeedbackPitch
                                                    .Get(fp => fp.PitchId == pitchId, includeProperties: "User") 
                                                    .ToListAsync();
            return _mapper.Map<List<FeedbackPitchDto>>(feedbackPitches);
        }

        public async Task<FeedbackPitchDto?> UpdateFeedbackPitchAsync(int feedbackId, FeedbackPitchUpdateDto feedbackPitchUpdateDTO)
        {
            var existingFeedbackPitch = await _unitOfWork.FeedbackPitch.GetByIdAsync(feedbackId);
            if (existingFeedbackPitch == null)
            {
                return null;
            }

            _mapper.Map(feedbackPitchUpdateDTO, existingFeedbackPitch);

            _unitOfWork.FeedbackPitch.Update(existingFeedbackPitch);
            await _unitOfWork.SaveChangesAsync();

            return await GetFeedbackPitchByIdAsync(existingFeedbackPitch.FeedbackId);
        }
    }
}

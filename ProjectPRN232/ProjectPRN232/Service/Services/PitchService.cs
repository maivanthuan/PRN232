using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.DTOs;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Hosting; // Add this namespace
using Microsoft.AspNetCore.Http; // Add this namespace
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using Service.IServices;

namespace Service.Services
{
    public class PitchService : IPitchServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment; // New field

        public PitchService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment; // Initialize
        }

        public async Task<PitchDTO> CreatePitchAsync(PitchCreateDTO pitchCreateDTO)
        {
            var pitch = _mapper.Map<Pitch>(pitchCreateDTO);

            if (pitchCreateDTO.ImageFile != null)
            {
                pitch.Image = await SaveImage(pitchCreateDTO.ImageFile);
            }

            var newPricePitch = new PricePitch
            {
                Price = pitchCreateDTO.Price,
                TimeStart = DateTime.UtcNow,
                TimeEnd = pitchCreateDTO.TimeEnd
            };

            pitch.PricePitches.Add(newPricePitch);
            pitch.PitchType = 5;
            await _unitOfWork.Pitch.AddAsync(pitch);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<PitchDTO>(pitch);
            return resultDto;
        }


        public async Task<bool> DeletePitchAsync(string pitchId)
        {
            var pitch = await _unitOfWork.Pitch.GetByIdAsync(pitchId);
            if (pitch == null) return false;
            pitch.Status = 0;

            _unitOfWork.Pitch.Update(pitch);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public IQueryable<PitchDTO> GetPitchAsQueryable()
        {
            var pitchs = _unitOfWork.Pitch.Get(includeProperties: "PricePitches");
            return pitchs.ProjectTo<PitchDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<PitchDTO?> GetPitchByIdAsync(string id)
        {
            var pitchs = await _unitOfWork.Pitch
                                     .Get(a => a.PitchId == id, includeProperties: "PricePitches")
                                     .FirstOrDefaultAsync();

            return _mapper.Map<PitchDTO>(pitchs);
        }

        public async Task<PitchDTO?> UpdatePitchAsync(string pitchId, PitchUpdateDTO pitchUpdateDTO)
        {
            var existingPitch = await _unitOfWork.Pitch.GetByIdAsync(pitchId);
            if (existingPitch == null)
            {
                return null;
            }
            var type = existingPitch.PitchType;
            existingPitch.PitchType = pitchUpdateDTO.PitchType;
            // existingPitch.Image = pitchUpdateDTO.Image; // This line should be removed or handled by file upload
            existingPitch.Status = pitchUpdateDTO.Status;

            if (pitchUpdateDTO.ImageFile != null)
            {
                // Delete old image if it exists
                if (!string.IsNullOrEmpty(existingPitch.Image))
                {
                    DeleteImage(existingPitch.Image);
                }
                existingPitch.Image = await SaveImage(pitchUpdateDTO.ImageFile);
            }

            var newPricePitch = new PricePitch
            {
                Price = pitchUpdateDTO.Price,
                TimeStart = DateTime.UtcNow,
                TimeEnd = pitchUpdateDTO.TimeEnd

            };
            existingPitch.PitchType = type;
            existingPitch.PricePitches.Add(newPricePitch);

            _unitOfWork.Pitch.Update(existingPitch);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PitchDTO>(existingPitch);
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img", "logo");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "/img/logo/" + uniqueFileName; // Return relative path
        }

        private void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}
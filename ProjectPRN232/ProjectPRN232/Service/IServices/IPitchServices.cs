using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;

namespace Service.IServices
{
    public interface IPitchServices
    {
        IQueryable<PitchDTO> GetPitchAsQueryable();
        Task<PitchDTO?> GetPitchByIdAsync(string id);
        Task<PitchDTO> CreatePitchAsync(PitchCreateDTO pitchCreateDTO);

        Task<PitchDTO?> UpdatePitchAsync(string pitchId, PitchUpdateDTO pitchUpdateDTO);

        Task<bool> DeletePitchAsync(string pitchId);
    }
}

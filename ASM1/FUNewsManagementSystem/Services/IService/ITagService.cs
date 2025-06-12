using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.DTO;

namespace Services.IService
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllAsync();
        Task<IEnumerable<TagDto>> GetUnusedAsync();
        Task<TagDto?> GetByIdAsync(int id);
        Task AddAsync(TagDto dto);
        Task<bool> UpdateAsync(int id, TagDto dto);
        Task<bool> DeleteAsync(int id);
        
    }
}

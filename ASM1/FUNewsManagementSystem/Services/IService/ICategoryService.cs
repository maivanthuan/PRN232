using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.DTO;

namespace Services.IService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(short id);
        Task AddAsync(CategoryDto dto);
        Task UpdateAsync(CategoryDto dto);
        Task<bool> DeleteAsync(short id);
    }
}

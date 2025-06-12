using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.DTO;

namespace Services.IService
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> GetActiveNewsAsync();
        Task<IEnumerable<NewsDto>> GetByAuthorAsync(short authorId);
        Task<NewsDto?> GetByIdAsync(String id);
        Task CreateNewsAsync(NewsCreateDto dto, short authorId);
        Task<bool> UpdateNewsAsync(String id, NewsCreateDto dto, short authorId);
        Task<bool> DeleteNewsAsync(String id, short requesterId);
       
    }
}

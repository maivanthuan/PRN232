using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.IRepository
{
    public interface INewsArticleRepository : IGenericRepository<NewsArticle>
    {
        Task<IEnumerable<NewsArticle>> GetActiveAsync();
        Task<IEnumerable<NewsArticle>> GetByAuthorAsync(short authorId);
    }
}

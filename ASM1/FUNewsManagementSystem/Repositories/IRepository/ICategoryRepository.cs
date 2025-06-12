using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Repositories.IRepository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> HasNewsArticlesAsync(short categoryId);
    }
}

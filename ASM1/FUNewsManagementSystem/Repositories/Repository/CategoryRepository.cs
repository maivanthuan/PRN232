using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FunewsManagementContext context) : base(context) { }

        public async Task<bool> HasNewsArticlesAsync(short categoryId)
            => await _context.NewsArticles.AnyAsync(n => n.CategoryId == categoryId);
    }
}

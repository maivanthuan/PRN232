using Ass1.Data;
using Ass1.Models;
using Ass1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ass1.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FUNewsManagementSystemContext _context;
        public CategoryRepository(FUNewsManagementSystemContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Category entity)
        {
            await  _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(object id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public Task UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            return _context.SaveChangesAsync();
        }
    }
}

using Ass1.Data;
using Ass1.Models;
using Ass1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ass1.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly FUNewsManagementSystemContext _context;

        public TagRepository(FUNewsManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(object id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task AddAsync(Tag entity)
        {
            await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag entity)
        {
            _context.Tags.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }
    }
}

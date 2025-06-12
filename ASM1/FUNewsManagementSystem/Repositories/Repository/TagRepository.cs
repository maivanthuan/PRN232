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
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(FunewsManagementContext context) : base(context) { }

        public async Task<IEnumerable<Tag>> GetUnusedTagsAsync()
            => await _context.Tags
                .Include(t => t.NewsArticles)
                .Where(t => !t.NewsArticles.Any())
                .ToListAsync();
    }
}

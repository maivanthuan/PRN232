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
    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FunewsManagementContext context) : base(context) { }

        public async Task<IEnumerable<NewsArticle>> GetActiveAsync()
            => await _context.NewsArticles
                   .Include(n => n.Category)
                   .Include(n => n.Tags)
                   .Where(n => n.NewsStatus == true)
                   .ToListAsync();

        public async Task<IEnumerable<NewsArticle>> GetByAuthorAsync(short authorId)
            => await _context.NewsArticles
                   .Include(n => n.Tags)
                   .Where(n => n.CreatedById == authorId)
                   .ToListAsync();
    }
}

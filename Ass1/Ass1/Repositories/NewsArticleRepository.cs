using Ass1.Data;
using Ass1.Models;
using Ass1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ass1.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FUNewsManagementSystemContext _context;

        public NewsArticleRepository(FUNewsManagementSystemContext context)
        {
            _context = context;
        }
        public async Task AddAsync(NewsArticle entity)
        {
            await _context.NewsArticles.AddAsync(entity);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(object id)
        {
            var article = await _context.NewsArticles.FindAsync(id);
            if (article != null)
            {
                _context.NewsArticles.Remove(article);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            return await _context.NewsArticles.Include(n => n.Category)
                                          .Include(n => n.Tags)
                                          .Include(n => n.CreatedBy)
                                          .ToListAsync();
        }

        public async Task<NewsArticle> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(NewsArticle entity)
        {
            _context.NewsArticles.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}

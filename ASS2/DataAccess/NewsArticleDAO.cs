using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class NewsArticleDAO
    {
        private readonly FunewsManagementContext _context;
        public NewsArticleDAO(FunewsManagementContext context) => _context = context;

        public IQueryable<NewsArticle> GetAll()
            => _context.NewsArticles
                       .Include(n => n.Category)
                       .Include(n => n.Tags); // OData-ready

        public NewsArticle? GetById(string id)
            => _context.NewsArticles
                       .Include(n => n.Category)
                       .Include(n => n.Tags)
                       .FirstOrDefault(n => n.NewsArticleId == id);

        public void Add(NewsArticle n)
        {
            _context.NewsArticles.Add(n);
            _context.SaveChanges();
        }

        public void Update(NewsArticle n)
        {
            _context.NewsArticles.Update(n);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var existing = _context.NewsArticles.Find(id);
            if (existing != null)
            {
                _context.NewsArticles.Remove(existing);
                _context.SaveChanges();
            }
        }
        public List<NewsArticle> SearchByKeyword(string keyword)
        {
            return _context.NewsArticles
                .Where(n =>
                    (n.NewsTitle ?? "").Contains(keyword) ||
                    (n.Headline ?? "").Contains(keyword) ||
                    (n.NewsContent ?? "").Contains(keyword))
                .Include(n => n.Category)
                .ToList();
        }

    }
}

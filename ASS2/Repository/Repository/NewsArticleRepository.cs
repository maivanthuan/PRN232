using BusinessObject.Models;
using DataAccess;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class NewsArticleRepository : INewsArticleRepository
        
    {
        private readonly NewsArticleDAO _newsArticleDAO;
        public NewsArticleRepository(NewsArticleDAO newsArticleDAO) => _newsArticleDAO = newsArticleDAO;
        public void Add(NewsArticle article) => _newsArticleDAO.Add(article);

        public void Delete(string id) => _newsArticleDAO.Delete(id);

        public IQueryable<NewsArticle> GetAll() => _newsArticleDAO.GetAll();

        public NewsArticle? GetById(string id) => _newsArticleDAO.GetById(id);

        public List<NewsArticle> SearchByKeyword(string keyword) => _newsArticleDAO.SearchByKeyword(keyword);

        public void Update(NewsArticle article) => _newsArticleDAO.Update(article);
    }
}

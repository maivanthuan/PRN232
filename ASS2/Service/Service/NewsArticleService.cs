using BusinessObject.Models;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _repo;
        public NewsArticleService(INewsArticleRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        public void Add(NewsArticle article) => _repo.Add(article);

        public void Delete(string id) => _repo.Delete(id);

        public IQueryable<NewsArticle> GetAll() => _repo.GetAll();

        public NewsArticle? GetById(string id) => _repo.GetById(id);

        public List<NewsArticle> Search(string keyword) => _repo.SearchByKeyword(keyword);

        public void Update(NewsArticle article) => _repo.Update(article);
    }
}

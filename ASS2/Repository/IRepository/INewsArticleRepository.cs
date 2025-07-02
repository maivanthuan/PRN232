using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface INewsArticleRepository
    {
        IQueryable<NewsArticle> GetAll();
        NewsArticle? GetById(string id);
        void Add(NewsArticle article);
        void Update(NewsArticle article);
        void Delete(string id);
        List<NewsArticle> SearchByKeyword(string keyword);

    }
}

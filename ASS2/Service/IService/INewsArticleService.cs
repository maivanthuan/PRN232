using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface INewsArticleService
    {
        IQueryable<NewsArticle> GetAll();
        NewsArticle? GetById(string id);
        void Add(NewsArticle article);
        void Update(NewsArticle article);
        void Delete(string id);
        List<NewsArticle> Search(string keyword);

    }
}

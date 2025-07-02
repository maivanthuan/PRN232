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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryDAO _categoryDAO;
        public CategoryRepository(CategoryDAO categoryDAO)
        {
            _categoryDAO = categoryDAO;
        }

        public void Add(Category category) => _categoryDAO.Add(category);

        public void Delete(int id) => _categoryDAO.Delete(id);

        public IQueryable<Category> GetAll() => _categoryDAO.GetAll();

        public Category? GetById(int id) => _categoryDAO.GetById(id);

        public void Update(Category category) => _categoryDAO.Update(category);
    }
}

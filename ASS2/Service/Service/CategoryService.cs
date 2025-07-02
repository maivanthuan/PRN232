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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }
        public void Add(Category category) => _repo.Add(category);

        public void Delete(int id) => _repo.Delete(id);

        public IQueryable<Category> GetAll()  => _repo.GetAll();

        public Category? GetById(int id) => _repo.GetById(id);

        public void Update(Category category) => _repo.Update(category);
    }
}

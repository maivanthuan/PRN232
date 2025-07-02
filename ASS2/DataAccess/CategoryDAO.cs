using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAO
    {
        private readonly FunewsManagementContext _context;
        public CategoryDAO(FunewsManagementContext context) => _context = context;

        public IQueryable<Category> GetAll() => _context.Categories;

        public Category? GetById(int id) => _context.Categories.Find(id);

        public void Add(Category c)
        {
            _context.Categories.Add(c);
            _context.SaveChanges();
        }

        public void Update(Category c)
        {
            _context.Categories.Update(c);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.Categories.Find(id);
            if (existing != null)
            {
                _context.Categories.Remove(existing);
                _context.SaveChanges();
            }
        }


    }
}

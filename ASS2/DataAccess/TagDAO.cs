using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TagDAO
    {
        private readonly FunewsManagementContext _context;
        public TagDAO(FunewsManagementContext context) => _context = context;

        public IQueryable<Tag> GetAll() => _context.Tags;

        public Tag? GetById(int id) => _context.Tags.Find(id);

        public void Add(Tag t)
        {
            _context.Tags.Add(t);
            _context.SaveChanges();
        }

        public void Update(Tag t)
        {
            _context.Tags.Update(t);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.Tags.Find(id);
            if (existing != null)
            {
                _context.Tags.Remove(existing);
                _context.SaveChanges();
            }
        }

    }
}

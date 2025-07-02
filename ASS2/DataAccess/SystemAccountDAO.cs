using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SystemAccountDAO
    {
        private readonly FunewsManagementContext _context;
        public SystemAccountDAO(FunewsManagementContext context) => _context = context;

        public IQueryable<SystemAccount> GetAll() => _context.SystemAccounts;

        public SystemAccount? GetById(int id) => _context.SystemAccounts.Find(id);

        public void Add(SystemAccount acc)
        {
            _context.SystemAccounts.Add(acc);
            _context.SaveChanges();
        }

        public void Update(SystemAccount acc)
        {
            _context.SystemAccounts.Update(acc);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _context.SystemAccounts.Find(id);
            if (existing != null)
            {
                _context.SystemAccounts.Remove(existing);
                _context.SaveChanges();
            }
        }

    }
}

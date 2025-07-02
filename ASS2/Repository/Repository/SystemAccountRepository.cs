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
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly SystemAccountDAO _systemAccountDAO;
        public SystemAccountRepository(SystemAccountDAO systemAccountDAO)
        {
            _systemAccountDAO = systemAccountDAO;
        }

        public void Add(SystemAccount account) => _systemAccountDAO.Add(account);

        public void Delete(int id) => _systemAccountDAO.Delete(id); 

        public IQueryable<SystemAccount> GetAll() => _systemAccountDAO.GetAll();

        public SystemAccount? GetById(int id) => _systemAccountDAO.GetById(id);

        public void Update(SystemAccount account) => _systemAccountDAO.Update(account);
    }
}

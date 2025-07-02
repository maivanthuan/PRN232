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
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _repo;
        public SystemAccountService(ISystemAccountRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        public void Add(SystemAccount account) => _repo.Add(account);

        public void Delete(int id) => _repo.Delete(id);

        public IQueryable<SystemAccount> GetAll() => _repo.GetAll();

        public SystemAccount? GetById(int id) => _repo.GetById(id);

        public void Update(SystemAccount account) => _repo.Update(account);
    }
}

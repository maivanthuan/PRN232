using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface ISystemAccountService
    {
        IQueryable<SystemAccount> GetAll();
        SystemAccount? GetById(int id);
        void Add(SystemAccount account);
        void Update(SystemAccount account);
        void Delete(int id);

    }
}

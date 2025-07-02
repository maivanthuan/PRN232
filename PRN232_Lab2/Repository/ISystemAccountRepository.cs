using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ISystemAccountRepository
    {
        Task<SystemAccount> Login(string email, string password);
    }
}

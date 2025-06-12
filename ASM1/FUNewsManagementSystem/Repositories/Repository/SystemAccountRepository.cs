using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
    {
        public SystemAccountRepository(FunewsManagementContext context) : base(context) { }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
            => await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == email);

        public async Task<IEnumerable<SystemAccount>> GetByRoleAsync(int role)
            => await _context.SystemAccounts.Where(a => a.AccountRole == role).ToListAsync();
    }
}

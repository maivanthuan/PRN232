using Ass1.Data;
using Ass1.Models;
using Ass1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ass1.Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly FUNewsManagementSystemContext _context;
        public SystemAccountRepository(FUNewsManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<SystemAccount> GetByEmailAsync(string email)
        {
            return await _context.SystemAccounts.FirstOrDefaultAsync(a => a.AccountEmail == email);
        }
        public async Task<bool> DeleteAsync(short id)
        {
            var account = await _context.SystemAccounts
                .Include(a => a.CreatedNewsArticles)
                .FirstOrDefaultAsync(a => a.AccountId == id);
            if (account == null) return false;

            if (account.CreatedNewsArticles.Any()) return false;

            _context.SystemAccounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<SystemAccount>> SearchAsync(string keyword)
        {
            return await _context.SystemAccounts
                .Where(a => a.AccountName.Contains(keyword) || a.AccountEmail.Contains(keyword))
                .ToListAsync();
        }
        public async Task AddAsync(SystemAccount entity)
        {
            await _context.SystemAccounts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(object id)
        {
            var account = await _context.SystemAccounts.FindAsync(id);
            if (account != null)
            {
                _context.SystemAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<SystemAccount>> GetAllAsync()
        {
            return await _context.SystemAccounts.ToListAsync();
        }
        public async Task<SystemAccount> GetByIdAsync(object id)
        {
            return await _context.SystemAccounts.FindAsync(id);
        }
        public Task UpdateAsync(SystemAccount entity)
        {
            _context.SystemAccounts.Update(entity);
            return _context.SaveChangesAsync();
        }
    }
    
    }


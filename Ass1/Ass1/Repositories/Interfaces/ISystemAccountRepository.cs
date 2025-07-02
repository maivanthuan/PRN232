using Ass1.Models;

namespace Ass1.Repositories.Interfaces
{
    public interface ISystemAccountRepository : IRepository<SystemAccount>
    {
        Task<SystemAccount> GetByEmailAsync(string email);
        Task<bool> DeleteAsync(short id); // Thêm phương thức đặc biệt
        Task<IEnumerable<SystemAccount>> SearchAsync(string keyword);
    }
}

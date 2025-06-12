using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.DTO;

namespace Services.IService
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAllAsync();
        Task<AccountDto?> GetByIdAsync(short id);
        Task<AccountDto?> GetByEmailAsync(string email);
        Task AddAsync(AccountCreateDto dto);
        Task UpdateAsync(short id, AccountCreateDto dto);
        Task<bool> DeleteAsync(short id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.DTOs;

namespace Service.IServices
{
    public interface IAccountService
    {
        IQueryable<AccountDTO> GetAccountsAsQueryable();
        Task<AccountDTO?> GetAccountByIdAsync(int id);
        Task<AccountDTO> CreateAccountAsync(AccountCreateDTO accountCreateDTO);
        Task<AccountDTO?> UpdateAccountAsync(int accountId, AccountUpdateDTO accountUpdateDTO);
        Task<bool> DeleteAccountAsync(int accountId);
        string? Authenticate(LoginDTO loginDTO, out UserDTO? accountDTO);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDto);

        Task<bool> ToggleBlockAsync(int userId);

    }
}

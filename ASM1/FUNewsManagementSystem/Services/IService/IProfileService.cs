using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessObjects.DTO;

namespace Services.IService
{
    public interface IProfileService
    {
        Task<AccountDto?> GetProfileByEmailAsync(string email);
        Task<bool> UpdateProfileAsync(string email, AccountCreateDto dto);
    }
}

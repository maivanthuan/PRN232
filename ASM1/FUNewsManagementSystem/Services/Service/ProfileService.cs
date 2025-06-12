using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessObjects.DTO;
using Repositories.IRepository;
using Services.IService;

namespace Services.Service
{
    public class ProfileService : IProfileService
    {
        private readonly ISystemAccountRepository _repo;
        private readonly IMapper _mapper;

        public ProfileService(ISystemAccountRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<AccountDto?> GetProfileByEmailAsync(string email)
        {
            var acc = await _repo.GetByEmailAsync(email);
            return acc == null ? null : _mapper.Map<AccountDto>(acc);
        }

        public async Task<bool> UpdateProfileAsync(string email, AccountCreateDto dto)
        {
            var acc = await _repo.GetByEmailAsync(email);
            if (acc == null) return false;

            acc.AccountName = dto.AccountName;
            acc.AccountPassword = dto.AccountPassword;
            _repo.Update(acc);
            return true;
        }
    }
}

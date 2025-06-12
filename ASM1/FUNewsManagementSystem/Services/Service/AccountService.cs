using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.Models;
using DataAccessObjects.DTO;
using Repositories.IRepository;
using Services.IService;
using Microsoft.EntityFrameworkCore;

namespace Services.Service
{
    public class AccountService : IAccountService
    {
        private readonly ISystemAccountRepository _repo;
        private readonly IMapper _mapper;
        private readonly FunewsManagementContext _context; // ✅ Inject DbContext

        public AccountService(ISystemAccountRepository repo, IMapper mapper, FunewsManagementContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAsync()
        {
            var accounts = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task<AccountDto?> GetByIdAsync(short id)
        {
            var acc = await _repo.GetByIdAsync(id);
            return acc == null ? null : _mapper.Map<AccountDto>(acc);
        }

        public async Task<AccountDto?> GetByEmailAsync(string email)
        {
            var acc = await _repo.GetByEmailAsync(email);
            return acc == null ? null : _mapper.Map<AccountDto>(acc);
        }

        public async Task AddAsync(AccountCreateDto dto)
        {
            var acc = _mapper.Map<SystemAccount>(dto);
            await _repo.AddAsync(acc);
            await _context.SaveChangesAsync(); // ✅ Ghi vào DB
        }

        public async Task UpdateAsync(short id, AccountCreateDto dto)
        {
            var acc = await _repo.GetByIdAsync(id);
            if (acc == null) return;

            acc.AccountName = dto.AccountName;
            acc.AccountEmail = dto.AccountEmail;
            acc.AccountRole = dto.AccountRole;
            acc.AccountPassword = dto.AccountPassword;

            _repo.Update(acc);
            await _context.SaveChangesAsync(); // ✅ Ghi vào DB
        }

        public async Task<bool> DeleteAsync(short id)
        {
            var acc = await _repo.GetByIdAsync(id);
            if (acc == null || acc.NewsArticles.Any()) return false;

            _repo.Delete(acc);
            await _context.SaveChangesAsync(); // ✅ Ghi vào DB
            return true;
        }
    }
}

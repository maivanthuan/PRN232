using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.DTOs;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepositories;
using Service.IServices;

namespace Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public AccountService(IGenericRepository<Account> userRepository, IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _accountRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<AccountDTO> CreateAccountAsync(AccountCreateDTO accountCreateDTO)
        {
            var account = _mapper.Map<Account>(accountCreateDTO);
            account.RoleId = 2;
            account.StatusOtp = 1;
            await _unitOfWork.Account.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();
            var createdAccount = await GetAccountByIdAsync(account.UserId);
            return createdAccount!;
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = await _unitOfWork.Account.GetByIdAsync(accountId);
            if (account == null) return false;

            _unitOfWork.Account.Delete(account);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public IQueryable<AccountDTO> GetAccountsAsQueryable()
        {
            var accounts = _unitOfWork.Account.Get();
            return accounts.ProjectTo<AccountDTO>(_mapper.ConfigurationProvider);
        }

        public async Task<AccountDTO?> GetAccountByIdAsync(int id)
        {
            var account = await _unitOfWork.Account
                                     .Get(a => a.UserId == id)
                                     .FirstOrDefaultAsync();

            return _mapper.Map<AccountDTO>(account);
        }

        public async Task<AccountDTO?> UpdateAccountAsync(int accountId, AccountUpdateDTO accountUpdateDTO)
        {
            var existingAccount = await _unitOfWork.Account.GetByIdAsync(accountId);
            if (existingAccount == null) return null;

            // LƯU TRỮ GIÁ TRỊ UserName GỐC TRƯỚC KHI ÁNH XẠ
            // Điều này đảm bảo UserName không bị ghi đè bằng NULL nếu DTO không cung cấp nó
            // hoặc DTO cung cấp nó dưới dạng NULL một cách không mong muốn.
            var originalUserName = existingAccount.UserName;
            
            _mapper.Map(accountUpdateDTO, existingAccount);
            existingAccount.UserName = originalUserName;
            existingAccount.RoleId = 2;
            _unitOfWork.Account.Update(existingAccount);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountDTO>(existingAccount);
        }

        public string? Authenticate(LoginDTO loginDTO, out UserDTO? userDTO)
        {
            userDTO = null;
            var account = _accountRepository.Get().FirstOrDefault(u => u.UserName == loginDTO.Username && u.Password == loginDTO.Password);
            if (account == null)
            {
                return null;
            }
            string role = account.RoleId switch
            {
                1 => "Admin",
                2 => "User",
                _ => "Unknown"
            };
            if (role == "Unknown")
            {
                return null; 
            }
            var token = GenerateToken(account.UserId, account.Email, role);
            userDTO = _mapper.Map<UserDTO>(account);
            userDTO.Role = role; 
            return token; 
        }
        private string GenerateToken(int userId, string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiresMinutes = double.Parse(jwtSettings["ExpiryMinutes"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                claims: claims,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDto)
        {
            var account = await _unitOfWork.Account.GetByIdAsync(userId);
            if (account == null)
            {
                return false; // User not found
            }

            // Verify current password (VERY IMPORTANT!)
            if (account.Password != changePasswordDto.CurrentPassword)
            {
                // Thay vì trả về false chung chung, hãy throw một ngoại lệ hoặc trả về một enum để phân biệt lỗi
                throw new ArgumentException("Mật khẩu hiện tại không đúng.");
            }

            // Update with the new password
            account.Password = changePasswordDto.Password;

            _unitOfWork.Account.Update(account);
            var result = await _unitOfWork.SaveChangesAsync();

            return result > 0;
        }
        public async Task<bool> ToggleBlockAsync(int userId)
        {
            var account = await _accountRepository.GetByIdAsync(userId);
            if (account == null) return false;

            account.StatusOtp = account.StatusOtp == 1 ? (byte?)0 : (byte?)1;

            _accountRepository.Update(account);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

    }
}

using Repository.Interfaces;
using Repository.Models;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountRepository _accountRepo;
        public UserService(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<User> GetByUserNameAsync(string fullname)
        {
            return await _accountRepo.GetByUserNameAsync(fullname);
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _accountRepo.GetByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _accountRepo.GetByUserIdAsync(userId);
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            return await _accountRepo.UpdateUserAsync(user);
        }
    }
}

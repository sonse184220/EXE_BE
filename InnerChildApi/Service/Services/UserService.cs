using Repository.Models;
using Repository.Repositories;

namespace Service.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByProfileIdAsync(string profileId);
        Task<User> GetUserByIdAsync(string userId);
        Task<int> UpdateUserAsync(User user);
        Task<User> GetByUserNameAsync(string fullname);
        Task<User> GetByEmailAsync(string email);
    }
    public class UserService : IUserService
    {
        private readonly IAccountRepository _accountRepo;
        public UserService(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public async Task<User> GetUserByProfileIdAsync(string profileId)
        {
            return await _accountRepo.GetUserByProfileIdAsync(profileId);
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

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _accountRepo.GetAllUsersAsync();
        }
    }
}

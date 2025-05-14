using Repository.Models;
namespace Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<User> GetByUserNameAsync(string fullname);
        Task<User> GetByEmailAsync(string email);
        Task<int> CreateUserAsync(User user);
        Task<User> GetByUserIdAsync(string userId);
        Task<int> UpdateUserAsync(User user);
        Task<User> LoginAsync(string email, string password);
        Task<List<Profile>> GetUserProfilesAsync(string userId);
    }
}

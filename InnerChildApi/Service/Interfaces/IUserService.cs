using Repository.Models;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<int> UpdateUserAsync(User user);
        Task<User> GetByUserNameAsync(string fullname);
        Task<User> GetByEmailAsync(string email);
    }
}

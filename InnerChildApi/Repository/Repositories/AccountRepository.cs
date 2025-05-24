using Contract.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Models;

namespace Repository.Repositories
{
    public interface IAccountRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByProfileIdAsync(string profileId);
        Task<User> GetByUserNameAsync(string fullname);
        Task<User> GetByEmailAsync(string email);
        Task<int> CreateUserAsync(User user);
        Task<User> GetByUserIdAsync(string userId);
        Task<int> UpdateUserAsync(User user);
        Task<User> LoginAsync(string email, string password);
        Task<List<Profile>> GetUserProfilesAsync(string userId);
    }
    public class AccountRepository : GenericRepository<User>, IAccountRepository
    {

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.Include(x => x.Role).ToListAsync() ?? new List<User>();
        }

        public async Task<User> GetByUserNameAsync(string fullname)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.FullName == fullname);
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<User> GetByUserIdAsync(string userId)
        {
            return await GetByIdAsync(userId);
        }
        public async Task<int> CreateUserAsync(User user)
        {
            return await CreateAsync(user);
        }
        public async Task<int> UpdateUserAsync(User user)
        {
            return await UpdateAsync(user);
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == password);
        }

        public async Task<List<Profile>> GetUserProfilesAsync(string userId)
        {
            return await _context.Profiles.Where(x => x.UserId == userId && x.ProfileStatus == UserAccountEnum.Active.ToString()).ToListAsync();
        }

        public async Task<User> GetUserByProfileIdAsync(string profileId)
        {
            var querry = from p in _context.Profiles
                         join u in _context.Users on p.UserId equals u.UserId
                         where p.ProfileId == profileId
                         select u;
            return await querry.FirstOrDefaultAsync();
        }
    }

}

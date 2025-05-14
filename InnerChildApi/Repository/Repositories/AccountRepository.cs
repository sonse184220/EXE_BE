using Contract.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.DBContext;
using Repository.Interfaces;
using Repository.Models;

namespace Repository.Repositories
{
    public class AccountRepository : GenericRepository<User>, IAccountRepository
    {
        public AccountRepository() : base()
        {
        }
        public AccountRepository(InnerChildExeContext context) : base(context)
        {


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
            return await base.GetByIdAsync(userId);
        }
        public async Task<int> CreateUserAsync(User user)
        {
            return await base.CreateAsync(user);
        }
        public async Task<int> UpdateUserAsync(User user)
        {
            return await base.UpdateAsync(user);
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == password);
        }

        public async Task<List<Profile>> GetUserProfilesAsync(string userId)
        {
            return await _context.Profiles.Where(x => x.UserId == userId && x.ProfileStatus == UserAccountEnum.Active.ToString()).ToListAsync();
        }
    }

}

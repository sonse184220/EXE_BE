using Contract.Dtos.Requests;
using Contract.Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.DBContext;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AccountRepository:GenericRepository<User>,IAccountRepository
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
    }
   
}

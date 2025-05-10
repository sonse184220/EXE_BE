using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

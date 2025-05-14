using Repository.Models;

namespace Repository.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByRoleNameAsync(string roleName);
    }
}

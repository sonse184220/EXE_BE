using Contract.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetByRoleNameAsync(string roleName);
        Task SeedRolesAsync();
    }
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        
        public async Task<Role> GetByRoleNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task SeedRolesAsync()
        {
            var existingRoles = await _context.Roles.Select(r => r.RoleName).ToListAsync();
            foreach (var roleName in Enum.GetNames(typeof(RoleEnum)))
            {
                if (!existingRoles.Contains(roleName))
                {
                    var role = new Role
                    {
                        RoleId = Guid.NewGuid().ToString(),
                        RoleName = roleName
                    };
                    _context.Roles.Add(role);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}

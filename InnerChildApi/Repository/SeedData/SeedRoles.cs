using Contract.Common.Enums;
using Repository.DBContext;

namespace Repository.SeedData
{
    public class SeedRoles
    {
        private readonly InnerChildExeContext _context;
        public SeedRoles(InnerChildExeContext context)
        {
            _context = context;
        }

        public void SeedRolesData()
        {
            var existingRoles = _context.Roles.Select(r => r.RoleName).ToList();
            foreach (var roleName in Enum.GetNames(typeof(RoleEnum)))
            {
                if (!existingRoles.Contains(roleName))
                {
                    var role = new Repository.Models.Role
                    {
                        RoleId = Guid.NewGuid().ToString(),
                        RoleName = roleName
                    };
                    _context.Roles.Add(role);
                }
            }
            _context.SaveChanges();
        }
    }

}

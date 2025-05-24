using Repository.Repositories;

namespace Repository.SeedData
{
    public class SeedRoles
    {
        private readonly IRoleRepository _roleRepo;
        public SeedRoles(IRoleRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task SeedRolesData()
        {
            await _roleRepo.SeedRolesAsync();
        }
    }

}

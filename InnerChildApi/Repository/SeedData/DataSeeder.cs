using Microsoft.Extensions.DependencyInjection;
using Repository.SeedData;

namespace Repository.DataSeeder
{
    public static class DataSeeder
    {
        public static void SeedAll(IServiceProvider serviceProvider)
        {
            var seedRoles = serviceProvider.GetRequiredService<SeedRoles>();
            seedRoles.SeedRolesData();
        }
    }
}

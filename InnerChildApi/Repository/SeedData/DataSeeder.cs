using Microsoft.Extensions.DependencyInjection;
using Repository.SeedData;

namespace Repository.DataSeeder
{
    public static class DataSeeder
    {
        public static async Task SeedAllAsync(IServiceProvider serviceProvider)
        {
            var seedRoles = serviceProvider.GetRequiredService<SeedRoles>();
            await seedRoles.SeedRolesData();
        }
    }
}

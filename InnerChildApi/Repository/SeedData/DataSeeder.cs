using Microsoft.Extensions.DependencyInjection;
using Repository.SeedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

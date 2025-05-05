using InnerChildApi.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Repository.DBContext;

namespace InnerChildApi
{
    public static class DependencyInjection
    {
       public static void AddApplicationConfiguration(this IServiceCollection services,IConfiguration config)
        {
            services.AddDbContext<InnerChildExeContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddHttpContextAccessor();
            services.AddCorsPolicy();
            services.AddJwtAuthentication(config);

           
        }
    }
}

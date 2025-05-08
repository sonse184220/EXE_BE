using Contract.Common.Config;
using InnerChildApi.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Repository.DBContext;
using System.Text.Json.Serialization;

namespace InnerChildApi
{
    public static class DependencyInjection
    {
       public static void AddApplicationConfiguration(this IServiceCollection services,IConfiguration config)
        {
            services.AddDbContext<InnerChildExeContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            //configure jwt settings
            services.Configure<AppSettingConfig.JwtTokenSetting>(config.GetSection("JwtSettings"));
            services.Configure<AppSettingConfig.CloudinarySettingConfig>(config.GetSection("CloudinarySettings"));
            services.Configure<AppSettingConfig.EmailSettingConfig>(config.GetSection("EmailSettings"));





            services.AddHttpContextAccessor();
            services.AddCorsPolicy();
            services.AddJwtAuthentication(config);
            //for enum
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            //auto mapper
            services.AddAutoMapper(typeof(Program).Assembly);

        }
    }
}

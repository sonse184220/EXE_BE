using Contract.Common.Config;
using Hangfire;
using InnerChildApi.Common.Configurations;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Serialization;

namespace InnerChildApi
{
    public static class DependencyInjection
    {
        public static void AddApplicationConfiguration(this IServiceCollection services, IConfiguration config)
        {

            //configure jwt settings
            services.Configure<AppSettingConfig.JwtTokenSetting>(config.GetSection("JwtSettings"));
            services.Configure<AppSettingConfig.CloudinarySettingConfig>(config.GetSection("CloudinarySettings"));
            services.Configure<AppSettingConfig.EmailSettingConfig>(config.GetSection("EmailSettings"));
            services.Configure<AppSettingConfig.AiSettingConfig>(config.GetSection("AiSettings"));
            services.Configure<AppSettingConfig.ChatDbSettingConfig>(config.GetSection("MongoDbSettings:ChatDb"));
            services.Configure<AppSettingConfig.SendGridSettingConfig>(config.GetSection("SendGridSettings"));
            services.Configure<AppSettingConfig.PayOsSettingConfig>(config.GetSection("PayOsSettings"));
            services.Configure<AppSettingConfig.AccountSeedingSettings>(config.GetSection("AccountSeedingSettings"));
            services.Configure<AppSettingConfig.QuizzSettingConfig>(config.GetSection("MongoDbSettings:QuizzDb"));


            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            services.AddJwtAuthentication(config);
            //for enum
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            //auto mapper
            services.AddAutoMapper(typeof(Program).Assembly);
            //support larger file upload
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; // 100 MB
            });
            services.AddHangfire(configuration => configuration
                .UseSqlServerStorage(config.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
            services.AddSignalR();


        }
    }
}

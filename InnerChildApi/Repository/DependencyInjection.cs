using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Repository.Repositories;
using Repository.SeedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection service)
        {
            service.AddScoped<SeedRoles>();
            service.AddScoped<IAccountRepository, AccountRepository>();
            service.AddScoped<IProfileRepository, ProfileRepository>();
            service.AddScoped<IRoleRepository, RoleRepository>();
            service.AddScoped<IArticleRepository, ArticleRepository>();
            service.AddScoped<ISessionRepository, SessionRepository>();
            service.AddScoped<INotificationRepository, NotificationRepository>();
            service.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            return service;
        }
    }
}

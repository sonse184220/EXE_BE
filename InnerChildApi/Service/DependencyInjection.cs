using Microsoft.Extensions.DependencyInjection;
using Service.Interfaces;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IEmailService, EmailService>();
            service.AddScoped<ICloudinaryImageService, CloudinaryImageService>();
            service.AddScoped<IContentService, ContentService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<INotificationService, NotificationService>();
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<ISessionService, SessionService>();

            return service;
        }
    }
}

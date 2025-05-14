using Microsoft.Extensions.DependencyInjection;
using Service.Interfaces;
using Service.Services;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<IEmailService, EmailService>();
            service.AddScoped<ICloudinaryService, CloudinaryService>();
            service.AddScoped<IContentService, ContentService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<INotificationService, NotificationService>();
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<ISessionService, SessionService>();
            service.AddScoped<IAudioService, AudioService>();
            service.AddScoped<IAiService, AiService>();
            service.AddScoped<ICommunityService,CommunityService>();
            return service;
        }
    }
}

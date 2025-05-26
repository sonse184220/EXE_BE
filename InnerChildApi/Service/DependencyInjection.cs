using Microsoft.Extensions.DependencyInjection;
using Service.BackgroundJobs;
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
            service.AddScoped<ICommunityService, CommunityService>();
            service.AddScoped<IMoodJournalService, MoodJournalService>();
            service.AddScoped<IChatService, ChatService>();
            service.AddScoped<IPaymentService, PaymentService>();
            service.AddScoped<ISubscriptionService, SubscriptionService>();
            service.AddScoped<IPurchaseService, PurchaseService>();
            service.AddScoped<IGoalService, GoalService>();



            #region hangfire jobs
            service.AddScoped<PurchaseCheckJob>();
            service.AddScoped<AuthSessionJob>();
            #endregion
            return service;
        }
    }
}

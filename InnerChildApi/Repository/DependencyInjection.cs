using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Repository.Repositories;
using Repository.SeedData;

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
            service.AddScoped<IAudioCategoryRepository, AudioCategoryRepository>();
            service.AddScoped<ISubAudioCategoryRepository, SubAudioCategoryRepository>();
            service.AddScoped<IAudioRepository, AudioRepository>();
            service.AddScoped<ICommunityGroupRepository, CommunityGroupRepository>();
            service.AddScoped<ICommunityMemberRepository, CommunityMemberRepository>();
            service.AddScoped<ICommunityPostRepository, CommunityPostRepository>();
            service.AddScoped<IMoodJournalTypeRepository, MoodJournalTypeRepository>();
            service.AddScoped<IMoodJournalRepository, MoodJournalRepository>();
            service.AddScoped<IChatRepository, ChatRepository>();
            return service;
        }
    }
}

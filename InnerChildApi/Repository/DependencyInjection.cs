using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Repository.SeedData;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection service)
        {
            #region seed data injection
            service.AddScoped<SeedRoles>();
            #endregion
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
            service.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            service.AddScoped<IPurchaseRepository, PurchaseRepository>();
            service.AddScoped<ITransactionRepository, TransactionRepository>();
            service.AddScoped<IGoalRepository, GoalRepository>();
            service.AddScoped<IHelpCategoryRepository, HelpCategoryRepository>();
            service.AddScoped<IHelpRepository, HelpRepository>();
            #region quizz
            service.AddScoped<IQuizCategoryRepository, QuizCategoryRepository>();
            service.AddScoped<IQuizzRepository, QuizzRepository>();
            service.AddScoped<IQuizzQuestionRepository, QuizzQuestionRepository>();
            service.AddScoped<IQuizzOptionRepository, QuizzOptionRepository>();

            #endregion
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();
            return service;
        }
    }
}

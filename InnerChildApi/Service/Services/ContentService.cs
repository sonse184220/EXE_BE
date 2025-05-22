using Repository.Models;
using Repository.Repositories;

namespace Service.Services
{
    public interface IContentService
    {
        //article
        Task<int> CreateArticleAsync(Article article);
        Task<List<Article>> GetAllArticlesAsync();
        Task<int> UpdateArticleAsync(Article article);
        Task<bool> DeleteArticleAsync(Article article);
        Task<Article> GetArticleByIdAsync(string id);

    }
    public class ContentService : IContentService
    {
        private readonly IArticleRepository _articleRepo;
        public ContentService(IArticleRepository articleRepo)
        {
            _articleRepo = articleRepo;
        }
        //article
        public async Task<int> CreateArticleAsync(Article article)
        {
            return await _articleRepo.CreateArticleAsync(article);
        }

        public async Task<bool> DeleteArticleAsync(Article article)
        {
            return await _articleRepo.DeleteArticleAsync(article);
        }

        public async Task<List<Article>> GetAllArticlesAsync()
        {
            return await _articleRepo.GetAllArticlesAsync();
        }

        public async Task<Article> GetArticleByIdAsync(string id)
        {
            return await _articleRepo.GetArticleByIdAsync(id);

        }

        public async Task<int> UpdateArticleAsync(Article article)
        {
            return await _articleRepo.UpdateArticleAsync(article);
        }
    }
}

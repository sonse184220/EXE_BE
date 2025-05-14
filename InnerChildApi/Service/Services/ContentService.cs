using Repository.Interfaces;
using Repository.Models;
using Service.Interfaces;

namespace Service.Services
{
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

using Repository.Base;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Repositories
{
    public interface IArticleRepository
    {
        Task<int> CreateArticleAsync(Article article);
        Task<List<Article>> GetAllArticlesAsync();
        Task<int> UpdateArticleAsync(Article article);
        Task<bool> DeleteArticleAsync(Article article);
        Task<Article> GetArticleByIdAsync(string id);

    }
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
      
        public async Task<int> CreateArticleAsync(Article article)
        {
            return await CreateAsync(article);
        }
        public async Task<List<Article>> GetAllArticlesAsync()
        {
            return await GetAllAsync();
        }
        public async Task<int> UpdateArticleAsync(Article article)
        {
            return await UpdateAsync(article);
        }
        public async Task<bool> DeleteArticleAsync(Article article)
        {
            return await RemoveAsync(article);
        }
        public async Task<Article> GetArticleByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }
    }

}

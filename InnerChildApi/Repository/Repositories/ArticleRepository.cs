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
        public ArticleRepository() : base()
        {

        }
        public ArticleRepository(InnerChildExeContext context) : base(context)
        {
        }
        public async Task<int> CreateArticleAsync(Article article)
        {
            return await base.CreateAsync(article);
        }
        public async Task<List<Article>> GetAllArticlesAsync()
        {
            return await base.GetAllAsync();
        }
        public async Task<int> UpdateArticleAsync(Article article)
        {
            return await base.UpdateAsync(article);
        }
        public async Task<bool> DeleteArticleAsync(Article article)
        {
            return await base.RemoveAsync(article);
        }
        public async Task<Article> GetArticleByIdAsync(string id)
        {
            return await base.GetByIdAsync(id);
        }
    }

}

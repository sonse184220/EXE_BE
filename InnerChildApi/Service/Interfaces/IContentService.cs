using Repository.Models;

namespace Service.Interfaces
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
}

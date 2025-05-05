using Repository.Base;
using Repository.DBContext;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ArticleRepository:GenericRepository<Article>, IArticleRepository
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

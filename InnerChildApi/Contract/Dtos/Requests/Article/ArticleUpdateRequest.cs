using Microsoft.AspNetCore.Http;

namespace Contract.Dtos.Requests.Article
{
    public class ArticleUpdateRequest
    {
        public string? ArticleName { get; set; }
        public string? ArticleDescription { get; set; }
        public string? ArticleContent { get; set; }
        public IFormFile? Image { get; set; }
    }
}

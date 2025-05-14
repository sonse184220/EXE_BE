using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Contract.Dtos.Requests.Article
{
    public class ArticleCreateRequest
    {
        [Required(ErrorMessage = "Article name is required.")]
        public string ArticleName { get; set; }
        [Required(ErrorMessage = "Article description is required.")]

        public string ArticleDescription { get; set; }
        [Required(ErrorMessage = "Article content is required.")]

        public string ArticleContent { get; set; }
        public IFormFile? Image { get; set; }
    }
}

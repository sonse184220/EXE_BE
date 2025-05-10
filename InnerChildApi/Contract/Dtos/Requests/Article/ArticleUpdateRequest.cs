using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

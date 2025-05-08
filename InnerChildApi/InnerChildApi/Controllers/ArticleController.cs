using Contract.Common.Enums;
using Contract.Dtos.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Interfaces;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly ICloudinaryImageService _cloudinaryImageService;
        public ArticleController(IContentService contentService, ICloudinaryImageService cloudinaryImageService)
        {
            _contentService = contentService;
            _cloudinaryImageService = cloudinaryImageService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateArticle([FromForm] CreateArticleRequest request)
        {
            try
            {
                var articleImageUrl = await _cloudinaryImageService.UploadImageAsync(request.Image, CloudinaryFolderEnum.Article.ToString());
                var article = new Article()
                {
                    ArticleId = Guid.NewGuid().ToString(),
                    ArticleName = request.ArticleName,
                    ArticleDescription = request.ArticleDescription,
                    ArticleContent = request.ArticleContent,
                    ArticleUrl = articleImageUrl
                };
                await _contentService.CreateArticleAsync(article);
                return Created("", new { message = "Article created successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500,$"Something went wrong "+ex.Message);
            }
           
        }
        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllArticle()
        {
            var articles=  await _contentService.GetAllArticlesAsync();
            return Ok(articles);

        }
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetArticleById(string id)
        {
            var article = await _contentService.GetArticleByIdAsync(id);
            if (article == null)
                return NotFound("Article not found");
            return Ok(article);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteArticle(string id)
        {
            var article = await _contentService.GetArticleByIdAsync(id);
            if (article == null)
                return NotFound("Article not found");
            try
            {
                var deleteResult = await _cloudinaryImageService.DeleteImageAsync(article.ArticleUrl);
                if (!deleteResult)
                {
                    return StatusCode(500, "Failed to delete image from Cloudinary");
                }
                var deleted = await _contentService.DeleteArticleAsync(article);
                if (deleted)
                {
                    return Ok("Article deleted successfully");
                }
                return StatusCode(500, "Failed to delete article");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateArticle(string id, [FromForm] UpdateArticleRequest updatedArticle)
        {
            var existingArticle = await _contentService.GetArticleByIdAsync(id);
            if (existingArticle == null)
            {
                return NotFound("Article not found");
            }
            if (updatedArticle.Image != null)
            {
                if (!string.IsNullOrEmpty(existingArticle.ArticleUrl))
                {
                    await _cloudinaryImageService.DeleteImageAsync(existingArticle.ArticleUrl);
                }
                existingArticle.ArticleUrl = await _cloudinaryImageService.UploadImageAsync(updatedArticle.Image, CloudinaryFolderEnum.Article.ToString());
            }
            existingArticle.ArticleName = updatedArticle.ArticleName ?? existingArticle.ArticleName;
            existingArticle.ArticleDescription = updatedArticle.ArticleDescription ?? existingArticle.ArticleDescription;
            existingArticle.ArticleContent = updatedArticle.ArticleContent ?? existingArticle.ArticleContent;

            await _contentService.UpdateArticleAsync(existingArticle);
            return Ok("Article updated successfully");
        }

    }
}

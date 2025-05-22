using Contract.Common.Enums;
using Contract.Dtos.Requests.Article;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Services;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly ICloudinaryService _cloudinaryService;
        public ArticleController(IContentService contentService, ICloudinaryService cloudinaryService)
        {
            _contentService = contentService;
            _cloudinaryService = cloudinaryService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (request.Image != null)
            {
                if (!request.Image.ContentType.StartsWith("image/"))
                {
                    return BadRequest($"{request.Image.FileName} is not image file");
                }
            }
            try
            {
                string articleImageUrl = null;
                if (request.Image != null)
                {
                    var articleImageParams = _cloudinaryService.CreateUploadParams(request.Image, CloudinaryFolderEnum.Article.ToString());
                    articleImageUrl = await _cloudinaryService.UploadAsync(articleImageParams, request.Image);
                }
                var article = new Article()
                {
                    ArticleId = Guid.NewGuid().ToString(),
                    ArticleName = request.ArticleName,
                    ArticleDescription = request.ArticleDescription,
                    ArticleContent = request.ArticleContent,
                    ArticleUrl = articleImageUrl.ToString()
                };
                await _contentService.CreateArticleAsync(article);
                return Created("", new { message = "Article created successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong " + ex.Message);
            }

        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllArticle()
        {
            var articles = await _contentService.GetAllArticlesAsync();
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
                var deleteResult = await _cloudinaryService.DeleteAsync(article.ArticleUrl);
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
        public async Task<IActionResult> UpdateArticle(string id, [FromForm] ArticleUpdateRequest updatedArticle)
        {

            if (updatedArticle.Image != null)
            {
                if (!updatedArticle.Image.ContentType.StartsWith("image/"))
                {
                    return BadRequest($"{updatedArticle.Image.FileName} is not image file");
                }
            }
            try
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
                        await _cloudinaryService.DeleteAsync(existingArticle.ArticleUrl);
                    }
                    var articleImageParams = _cloudinaryService.CreateUploadParams(updatedArticle.Image, CloudinaryFolderEnum.Article.ToString());
                    existingArticle.ArticleUrl = await _cloudinaryService.UploadAsync(articleImageParams, updatedArticle.Image);
                }
                existingArticle.ArticleName = updatedArticle.ArticleName ?? existingArticle.ArticleName;
                existingArticle.ArticleDescription = updatedArticle.ArticleDescription ?? existingArticle.ArticleDescription;
                existingArticle.ArticleContent = updatedArticle.ArticleContent ?? existingArticle.ArticleContent;

                await _contentService.UpdateArticleAsync(existingArticle);
                return Ok("Article updated successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

    }
}

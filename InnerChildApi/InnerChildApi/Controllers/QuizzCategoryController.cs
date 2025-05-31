using Contract.Dtos.Requests.Quizz;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Repository.MongoDbModels;
using Service.Services;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/quizz-category")]
    [ApiController]
    public class QuizzCategoryController : ControllerBase
    {
        private readonly ILogger<QuizzCategoryController> _logger;
        private readonly IQuizzService _quizzService;
        public QuizzCategoryController(ILogger<QuizzCategoryController> logger, IQuizzService quizzService)
        {
            _logger = logger;
            _quizzService = quizzService;
        }
        [HttpPost("create-quizz-category")]
        public async Task<IActionResult> CreateQuizzCategory([FromBody] QuizzCreateRequest.CreateQuizzCategoryDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var quizzCategory = new QuizzCategoryMongo()
            {
                QuizCategoryDescription = request.QuizCategoryDescription,
                QuizCategoryId = ObjectId.GenerateNewId().ToString(),
                QuizCategoryName = request.QuizCategoryName,
            };
            try
            {
                await _quizzService.CreateQuizzCategoryAsync(quizzCategory);
                return Created("", "Quiz created");

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("all-quizz-category")]
        public async Task<IActionResult> GetAllQuizzCategory()
        {
            var result = await _quizzService.GetAllQuizzCategoryAsync();
            return Ok(result);

        }
        [HttpGet("detail/{quizCategoryId}")]
        public async Task<IActionResult> GetQuizCategoryById(string quizCategoryId)
        {
            var quizCategory = await _quizzService.GetQuizzCategoryByIdAsync(quizCategoryId);
            if (quizCategory == null)
            {
                return NotFound();
            }
            return Ok(quizCategory);
        }
        [HttpPut("update/{quizCategoryId}")]
        public async Task<IActionResult> UpdateQuizzCategory(string quizCategoryId, [FromBody] QuizzCategoryUpdateRequest request)
        {
            var existingQuizzCategory = await _quizzService.GetQuizzCategoryByIdAsync(quizCategoryId);
            if (existingQuizzCategory == null)
            {
                return NotFound();
            }
            var quizCategory = new QuizzCategoryMongo()
            {
                QuizCategoryDescription = request.QuizCategoryDescription,
                QuizCategoryName = request.QuizCategoryName,
            };
            try
            {
            await _quizzService.UpdateQuizzCategoryAsync(quizCategoryId, quizCategory);
            return NoContent();
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

    }
}

using Contract.Dtos.Requests.Quizz;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InnerChildApi.Controllers
{
    [Route("innerchild/quizz")]
    [ApiController]
    public class QuizzController : ControllerBase
    {
        private readonly ILogger<QuizzController> _logger;
        private readonly IQuizzService _quizzService;
        public QuizzController(ILogger<QuizzController> logger, IQuizzService quizzService)
        {
            _logger = logger;
            _quizzService = quizzService;
        }
        [HttpPost("create-quizz")]
        public async Task<IActionResult> CreateQuizz([FromBody] QuizzCreateRequest.CreateQuizzDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _quizzService.CreateQuizzTransacAsync(request);
                return Created("", "Quiz created");

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
        [HttpGet("all-quizz")]
        public async Task<IActionResult> GetAllQuizz()
        {
            var allQuizzes = await _quizzService.GetAllQuizzAsync();
            return Ok(allQuizzes);

        }
        [HttpGet("get-quizz-detail/{quizzId}")]
        public async Task<IActionResult> GetQuizzDetailById(string quizzId)
        {
            if (string.IsNullOrEmpty(quizzId))
            {
                return BadRequest("Quizz ID is required");
            }
            try
            {
                var result = await _quizzService.GetFullQuizDetailByIdAsync(quizzId);
                if (result == null)
                {
                    return NotFound($"Quizz with ID {quizzId} not found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving quizz detail");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}

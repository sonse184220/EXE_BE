using Contract.Dtos.Requests.HelpAndAnswer;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InnerChildApi.Controllers
{
    [Route("innerchild/help")]
    [ApiController]
    public class HelpController : ControllerBase
    {
        private readonly IHelpAndAnswerService _helpAndAnswerService;
        private readonly ILogger<HelpController> _logger;
        public HelpController(IHelpAndAnswerService helpAndAnswerService, ILogger<HelpController> logger)
        {
            _helpAndAnswerService = helpAndAnswerService;
            _logger = logger;
        }
        [HttpGet("all-helpcategory")]
        public async Task<IActionResult> GetAllHelpCategory()
        {
            var result = await _helpAndAnswerService.GetAllHelpCategoryAsync();
            return Ok(result);
        }

        [HttpGet("helpcategory/detail/{id}")]
        public async Task<IActionResult> GetHelpCategoryById(string id)
        {
            var result = await _helpAndAnswerService.GetHelpCategoryByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("helpcategory/create")]
        public async Task<IActionResult> CreateHelpCategory([FromBody] HelpCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var helpCategory = new HelpCategory()
            {
                HelpCategoryId = Guid.NewGuid().ToString(),
                HelpCategoryName = request.HelpCategoryName,
            };
            try
            {
                var result = await _helpAndAnswerService.CreateHelpCategoryAsync(helpCategory);
                return Created("", "Help category created!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Error occured");
            }
        }

        [HttpPut("helpcategory/update")]
        public async Task<IActionResult> UpdateHelpCategory([FromBody] HelpCategory category)
        {
            var result = await _helpAndAnswerService.UpdateHelpCategoryAsync(category);
            return Ok(result);
        }

        [HttpDelete("helpcategory/delete/{id}")]
        public async Task<IActionResult> DeleteHelpCategory(string id)
        {
            var category = await _helpAndAnswerService.GetHelpCategoryByIdAsync(id);
            if (category == null) return NotFound();

            var result = await _helpAndAnswerService.DeleteHelpCategoryAsync(category);
            return Ok(result);
        }
    }
}

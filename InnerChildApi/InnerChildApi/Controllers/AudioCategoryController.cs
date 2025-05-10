using Contract.Dtos.Requests.Audio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Interfaces;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/audiocategory")]
    [ApiController]
    public class AudioCategoryController : ControllerBase
    {
        private readonly IAudioService _audioService;

        public AudioCategoryController(IAudioService audioService)
        {
            _audioService = audioService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAudioCategories()
        {
            var categories = await _audioService.GetAllAudioCategoryAsync();
            return Ok(categories);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetAudioCategoryById(string id)
        {
            var category = await _audioService.GetAudioCategoryByIdAsync(id);
            if (category == null) { 
            return NotFound();
            }
            return Ok(category);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAudioCategory([FromBody] AudioCategoryRequest request)
        {
            var audioCategory = new AudioCategory()
            {
                AudioCategoryId = Guid.NewGuid().ToString(),
                AudioCategoryName = request.AudioCategoryName,
            };
            var result = await _audioService.CreateAudioCategoryAsync(audioCategory);
            return Created("", new { message = "Audio category created successfully" });

        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAudioCategory(string id,[FromBody] AudioCategoryRequest request)
        {
            var existingAudioCategory = await _audioService.GetAudioCategoryByIdAsync(id);
            if (existingAudioCategory == null)
            {
                return NotFound();
            }
            existingAudioCategory.AudioCategoryName = request.AudioCategoryName;
            var updatedRows = await _audioService.UpdateAudioCategoryAsync(existingAudioCategory);
            if (updatedRows <= 0) return StatusCode(500);

            return NoContent();
        }

       
    }
}

using Contract.Dtos.Requests.Audio;
using Contract.Dtos.Responses.Audio;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Services;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/subaudiocategory")]
    [ApiController]
    public class SubAudioCategoryController : ControllerBase
    {
        private readonly IAudioService _audioService;
        public SubAudioCategoryController(IAudioService audioService)
        {
            _audioService = audioService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSubAudioCategories()
        {
            var subCategories = await _audioService.GetAllSubAudioCategoryAsync();
            var response = subCategories.Select(s => new SubAudioCategoryResponse
            {
                SubAudioCategoryId = s.SubAudioCategoryId,
                SubAudioCategoryName = s.SubAudioCategoryName,
                AudioCategoryId = s.AudioCategoryId,
                AudioCategoryName = s.AudioCategory.AudioCategoryName
            }).ToList();

            return Ok(response);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetSubAudioCategoryById(string id)
        {
            var subCategory = await _audioService.GetSubAudioCategoryByIdAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }
            var response = new SubAudioCategoryResponse
            {
                SubAudioCategoryId = subCategory.SubAudioCategoryId,
                SubAudioCategoryName = subCategory.SubAudioCategoryName,
                AudioCategoryId = subCategory.AudioCategoryId,
                AudioCategoryName = subCategory.AudioCategory?.AudioCategoryName
            };
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSubAudioCategory([FromBody] SubAudioCategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var audioCategory = await _audioService.GetAudioCategoryByIdAsync(request.AudioCategoryId);
            if (audioCategory == null)
            {
                return NotFound($"Audio category {request.AudioCategoryId} id not found");
            }
            var subAudioCategory = new SubAudioCategory()
            {
                SubAudioCategoryId = Guid.NewGuid().ToString(),
                SubAudioCategoryName = request.SubAudioCategoryName,
                AudioCategoryId = request.AudioCategoryId,
            };
            var result = await _audioService.CreateSubAudioCategoryAsync(subAudioCategory);
            return Created("", new { message = "Sub audio category created successfully" });

        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateSubAudioCategory(string id, [FromBody] SubAudioCategoryUpdateRequest updatedSubAudio)
        {
            var existingSubAudioCategory = await _audioService.GetSubAudioCategoryByIdAsync(id);
            if (existingSubAudioCategory == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(updatedSubAudio.AudioCategoryId))
            {
                var audioCategory = await _audioService.GetAudioCategoryByIdAsync(updatedSubAudio.AudioCategoryId);
                if (audioCategory == null)
                {
                    return NotFound($"Audio category with ID {updatedSubAudio.AudioCategoryId} not found.");
                }
                existingSubAudioCategory.AudioCategoryId = updatedSubAudio.AudioCategoryId;
            }
            existingSubAudioCategory.SubAudioCategoryName = updatedSubAudio.SubAudioCategoryName ?? existingSubAudioCategory.SubAudioCategoryName;
            var updatedRows = await _audioService.UpdateSubAudioSubCategoryAsync(existingSubAudioCategory);
            if (updatedRows <= 0) return StatusCode(500);
            return NoContent();
        }

    }
}

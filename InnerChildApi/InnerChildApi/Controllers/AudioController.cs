using Contract.Common.Enums;
using Contract.Dtos.Requests.Article;
using Contract.Dtos.Requests.Audio;
using Contract.Dtos.Responses.Audio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Interfaces;
using Service.Services;

namespace InnerChildApi.Controllers
{
    [Route("innerchild/audio")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IAudioService _audioService;
        public AudioController(ICloudinaryService cloudinaryService, IAudioService audioService)
        {
            _cloudinaryService = cloudinaryService;
            _audioService = audioService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAudio([FromForm] AudioCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (request.AudioFile != null)
            {
                var isAudio = request.AudioFile.ContentType.StartsWith("audio/");
                var isVideo = request.AudioFile.ContentType.StartsWith("video/");
                if (!(isAudio || isVideo))
                {
                    return BadRequest($"Only audio or video files are allowed for {request.AudioFile.FileName}.");
                }
            }
            if (request.AudioThumbnailFile != null)
            {
                var isImage = request.AudioThumbnailFile.ContentType.StartsWith("image/");
                if (!isImage)
                {
                    return BadRequest($"Only image files are allowed for {request.AudioThumbnailFile.FileName}.");
                }
            }
            
            try
            {
                var subAudioCategory = await _audioService.GetSubAudioCategoryByIdAsync(request.SubAudioCategoryId);
                if (subAudioCategory == null)
                {
                    return NotFound($"Sub category id {request.SubAudioCategoryId} not found");
                }
                string audioFileUrl = null;
                string audioThumbnailUrl = null;
                if (request.AudioFile != null)
                {
                    var audioParams = _cloudinaryService.CreateUploadParams(request.AudioFile, CloudinaryFolderEnum.Audio.ToString());
                    audioFileUrl = await _cloudinaryService.UploadAsync(audioParams, request.AudioFile);
                }
               if (request.AudioThumbnailFile != null)
                {
                    var audioThumbnailParams = _cloudinaryService.CreateUploadParams(request.AudioThumbnailFile, CloudinaryFolderEnum.AudioThumbnail.ToString());
                    audioThumbnailUrl = await _cloudinaryService.UploadAsync(audioThumbnailParams, request.AudioThumbnailFile);
                }
                var audio = new Audio()
                {
                    AudioId = Guid.NewGuid().ToString(),
                    AudioTitle = request.AudioTitle,
                    AudioUrl = audioFileUrl,
                    AudioThumbnail = audioThumbnailUrl,
                    AudioCreatedAt = DateTime.UtcNow,
                    AudioIsPremium = request.AudioIsPremium,
                    AudioStatus = request.AudioStatus.ToString(),
                    SubAudioCategoryId = subAudioCategory.SubAudioCategoryId,
                };
                await _audioService.CreateAudioAsync(audio);

                return Created("", new { message = "Audio created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAudio()
        {
            var result = await _audioService.GetAllAudioAsync();
            return Ok(result);

        }
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetAudioById(string id)
        {
            var audio = await _audioService.GetAudioByIdAsync(id);
            if (audio == null)
                return NotFound("Audio not found");
            var result = new AudioResponse()
            {
                AudioId = audio.AudioId,
                AudioTitle = audio.AudioTitle,
                AudioUrl = audio.AudioUrl,
                AudioThumbnail = audio.AudioThumbnail,
                AudioCreatedAt = audio.AudioCreatedAt,
                AudioIsPremium = audio.AudioIsPremium,
                AudioStatus = audio.AudioStatus,
                SubAudioCategoryId = audio.SubAudioCategoryId,
                SubAudioCategoryName = audio.SubAudioCategory?.SubAudioCategoryName
            };
            return Ok(result);
        }
        
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAudio(string id, [FromForm] AudioUpdateRequest updatedAudio)
        {
            if (updatedAudio.AudioThumbnailFile!= null)
            {
                if (!updatedAudio.AudioThumbnailFile.ContentType.StartsWith("image/"))
                {
                    return BadRequest($"{updatedAudio.AudioThumbnailFile.FileName} is not image file");
                }
            }
            if (updatedAudio.AudioFile != null)
            {
                var isAudio = updatedAudio.AudioFile.ContentType.StartsWith("audio/");
                var isVideo = updatedAudio.AudioFile.ContentType.StartsWith("video/");
                if (!(isAudio || isVideo))
                {
                    return BadRequest($"Only audio or video files are allowed for {updatedAudio.AudioFile.FileName}.");
                }
            }
            try
            {
                var existingAudio = await _audioService.GetAudioByIdAsync(id);
                
                if (existingAudio == null)
                {
                    return NotFound($"Audio with id {id} not found");
                }
                if (!string.IsNullOrWhiteSpace(updatedAudio.SubAudioCategoryId)){
                var existingSubAudioCategory = await _audioService.GetSubAudioCategoryByIdAsync(updatedAudio.SubAudioCategoryId);
                if (existingSubAudioCategory==null) {
                    return NotFound($"Audio with sub category id {updatedAudio.SubAudioCategoryId} not found");
                }
                    existingAudio.SubAudioCategoryId = updatedAudio.SubAudioCategoryId;
                }
                if (updatedAudio.AudioThumbnailFile != null)
                {
                    if (!string.IsNullOrEmpty(existingAudio.AudioThumbnail))
                    {
                        await _cloudinaryService.DeleteAsync(existingAudio.AudioThumbnail);
                    }
                    var audioThumbnailFileParams = _cloudinaryService.CreateUploadParams(updatedAudio.AudioThumbnailFile, CloudinaryFolderEnum.AudioThumbnail.ToString());
                    existingAudio.AudioThumbnail = await _cloudinaryService.UploadAsync(audioThumbnailFileParams, updatedAudio.AudioThumbnailFile);
                }
                if (updatedAudio.AudioFile != null)
                {
                    if (!string.IsNullOrEmpty(existingAudio.AudioUrl))
                    {
                        await _cloudinaryService.DeleteAsync(existingAudio.AudioUrl);
                    }
                    var audioFileParams = _cloudinaryService.CreateUploadParams(updatedAudio.AudioFile, CloudinaryFolderEnum.Audio.ToString());
                    existingAudio.AudioUrl = await _cloudinaryService.UploadAsync(audioFileParams, updatedAudio.AudioFile);
                }

                existingAudio.AudioTitle = updatedAudio.AudioTitle ?? existingAudio.AudioTitle;
                existingAudio.AudioIsPremium = updatedAudio.AudioIsPremium ?? existingAudio.AudioIsPremium;
                if (updatedAudio.AudioStatus.HasValue)
                {
                    existingAudio.AudioStatus = updatedAudio.AudioStatus.Value.ToString();
                }
                await _audioService.UpdateAudioAsync(existingAudio);
                return Ok("Audio updated successfully");
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

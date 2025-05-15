using Contract.Common.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Audio
{
    public class AudioCreateRequest
    {
        public string? AudioTitle { get; set; }
        [Required(ErrorMessage = "Audio status is required")]
        public AudioEnum AudioStatus { get; set; }
        public IFormFile? AudioFile { get; set; }
        public IFormFile? AudioThumbnailFile { get; set; }
        [Required(ErrorMessage = "Audio premium is required")]
        public bool AudioIsPremium { get; set; }

        [Required(ErrorMessage = "Sub audio category id is required")]
        public string SubAudioCategoryId { get; set; }

    }
}

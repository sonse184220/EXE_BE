using Contract.Common.Enums;
using Microsoft.AspNetCore.Http;

namespace Contract.Dtos.Requests.Audio
{
    public class AudioUpdateRequest
    {
        public string? AudioTitle { get; set; }
        public AudioEnum? AudioStatus { get; set; }
        public IFormFile? AudioFile { get; set; }
        public IFormFile? AudioThumbnailFile { get; set; }
        public bool? AudioIsPremium { get; set; }
        public string? SubAudioCategoryId { get; set; }
    }
}

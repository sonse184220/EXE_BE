using Contract.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Requests.Audio
{
    public class AudioCreateRequest
    {
        public string? AudioTitle { get; set; }
        [Required]
        public AudioEnum AudioStatus { get; set; }
        public IFormFile? AudioFile { get; set; }
        public IFormFile? AudioThumbnailFile { get; set; }
        [Required]
        public bool AudioIsPremium { get; set; }
        [Required]
        public string SubAudioCategoryId { get; set; }

    }
}

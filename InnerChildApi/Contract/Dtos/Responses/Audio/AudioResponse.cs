using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Responses.Audio
{
    public class AudioResponse
    {
        public string AudioId { get; set; }

        public string AudioTitle { get; set; }

        public string AudioUrl { get; set; }

        public string AudioThumbnail { get; set; }

        public DateTime? AudioCreatedAt { get; set; }

        public bool? AudioIsPremium { get; set; }

        public string AudioStatus { get; set; }

        public string SubAudioCategoryId { get; set; }
        public string SubAudioCategoryName { get; set; }

    }
}

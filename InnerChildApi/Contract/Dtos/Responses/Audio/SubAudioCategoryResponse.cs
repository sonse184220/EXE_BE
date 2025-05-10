using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Responses.Audio
{
    public class SubAudioCategoryResponse
    {
        public string SubAudioCategoryId { get; set; }
        public string SubAudioCategoryName { get; set; }
        public string AudioCategoryId { get; set; }
        public string AudioCategoryName { get; set; }
    }
}

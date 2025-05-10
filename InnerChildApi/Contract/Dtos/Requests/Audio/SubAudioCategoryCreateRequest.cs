using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Requests.Audio
{
    public class SubAudioCategoryCreateRequest
    {
        public string SubAudioCategoryName { get; set; }
        public string AudioCategoryId { get; set; }

    }
}

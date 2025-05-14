using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Requests.Community
{
    public class CommunityPostUpdateRequest
    {
        public string? CommunityPostTitle { get; set; }

        public string? CommunityPostContent { get; set; }

        public IFormFile? CommunityPostImageFile { get; set; }
        public string CommunityGroupId { get; set; }
    }
}

using Microsoft.AspNetCore.Http;

namespace Contract.Dtos.Requests.Community
{
    public class CommunityPostCreateRequest
    {

        public string CommunityPostTitle { get; set; }

        public string CommunityPostContent { get; set; }

        public IFormFile? CommunityPostImageFile { get; set; }
        public string CommunityGroupId { get; set; }

    }
}

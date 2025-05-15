using Contract.Common.Enums;

namespace Contract.Dtos.Requests.Community
{
    public class CommunityUpdateRequest
    {
        public string? CommunityName { get; set; }

        public string? CommunityDescription { get; set; }

        public CommunityEnum? CommunityStatus { get; set; }
    }
}

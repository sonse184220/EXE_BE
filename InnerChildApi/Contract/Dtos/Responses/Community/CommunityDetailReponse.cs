namespace Contract.Dtos.Responses.Community
{
    public class CommunityDetailReponse
    {
        public string CommunityGroupId { get; set; }

        public string CommunityName { get; set; }

        public string CommunityDescription { get; set; }

        public DateTime? CommunityCreatedAt { get; set; }

        public string CommunityStatus { get; set; }
        public IEnumerable<CommunityMemberDetail> CommunityMembersDetail { get; set; }
        public IEnumerable<CommunityPostDetail> CommunityPostsDetail { get; set; }






    }
    public class CommunityMemberDetail
    {
        public string CommunityMemberId { get; set; }

        public string CommunityMemberStatus { get; set; }

        public string CommunityGroupId { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProfilePicture { get; set; }
        public string ProfileId { get; set; }
    }

    public class CommunityPostDetail
    {
        public string CommunityPostId { get; set; }

        public string CommunityPostTitle { get; set; }

        public string CommunityPostContent { get; set; }

        public string CommunityPostImageUrl { get; set; }

        public string CommunityPostStatus { get; set; }

        public DateTime? CommunityPostCreatedAt { get; set; }

        public string CommunityGroupId { get; set; }

        public string UserId { get; set; }

        public string ProfileId { get; set; }

        public string UserName { get; set; }
        public string ProfilePicture { get; set; }
    }
}

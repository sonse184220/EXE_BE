namespace Contract.Dtos.Responses.Auth
{
    public class AllUsersResponse
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePicture { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string Status { get; set; }

        public bool? Verified { get; set; }

        public string RoleName { get; set; }

    }
}

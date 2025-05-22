namespace Contract.Dtos.Responses.Auth
{
    public class UserDetailResponse
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePicture { get; set; }

    }
}

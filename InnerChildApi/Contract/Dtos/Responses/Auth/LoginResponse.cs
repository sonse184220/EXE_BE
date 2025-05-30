﻿namespace Contract.Dtos.Responses.Auth
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<UserProfileDto> Profiles { get; set; }

    }
    public class UserProfileDto
    {
        public string ProfileId { get; set; }
        public string ProfileStatus { get; set; }
    }
}

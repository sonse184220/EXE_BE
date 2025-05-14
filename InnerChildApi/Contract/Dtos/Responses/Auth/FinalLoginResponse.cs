namespace Contract.Dtos.Responses.Auth
{
    public class FinalLoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }


    public class PreFinalLoginResponse
    {
        public string UserId { get; set; }
        public string ProfileId { get; set; }
    }
}

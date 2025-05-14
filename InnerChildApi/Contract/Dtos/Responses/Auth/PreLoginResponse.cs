namespace Contract.Dtos.Responses.Auth
{
    public class PreLoginResponse
    {
        public string UserId { get; set; }
        public string ProfileId { get; set; }
        public string ProfileStatus { get; set; }
        public string ProfileToken { get; set; }
    }
}

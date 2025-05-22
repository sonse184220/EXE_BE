namespace Contract.Dtos.Responses.Chat
{
    public class AllSessionResponse
    {

        public string AichatSessionId { get; set; }

        public string AichatSessionTitle { get; set; }

        public DateTime? AichatSessionCreatedAt { get; set; }

        public string ProfileId { get; set; }
    }
}

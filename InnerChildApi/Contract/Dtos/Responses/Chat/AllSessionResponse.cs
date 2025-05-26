namespace Contract.Dtos.Responses.Chat
{
    public class AllSessionResponse
    {

        public string AiChatSessionId { get; set; }

        public string AiChatSessionTitle { get; set; }

        public DateTime? AiChatSessionCreatedAt { get; set; }

        public string ProfileId { get; set; }
    }
}

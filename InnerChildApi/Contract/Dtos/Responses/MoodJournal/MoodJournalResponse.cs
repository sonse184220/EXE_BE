namespace Contract.Dtos.Responses.MoodJournal
{
    public class MoodJournalResponse
    {
        public string MoodJournalId { get; set; }

        public string MoodJournalTitle { get; set; }

        public string MoodJournalEmotion { get; set; }

        public string MoodJournalDescription { get; set; }

        public DateTime? MoodJournalCreatedAt { get; set; }

        public string MoodJournalTypeId { get; set; }

        public string ProfileId { get; set; }

        public string MoodJournalTypeName { get; set; }
    }
}

using Contract.Common.Enums;

namespace Contract.Dtos.Requests.MoodJournal
{
    public class MoodJournalUpdateRequest
    {
        public string? MoodJournalTitle { get; set; }
        public MoodJournalEnum? MoodJournalEmotion { get; set; }
        public string? MoodJournalDescription { get; set; }
        public string? MoodJournalTypeId { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.MoodJournal
{
    public class MoodJournalTypeCreateRequest
    {
        [Required(ErrorMessage = "Mood journal name is required")]
        public string MoodJournalTypeName { get; set; }
    }
}

using Contract.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.MoodJournal
{
    public class MoodJournalCreateRequest
    {
        [Required(ErrorMessage = "Title is required")]
        public string MoodJournalTitle { get; set; }
        [Required(ErrorMessage = "Mood journal emotion is required")]
        public MoodJournalEnum MoodJournalEmotion { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string MoodJournalDescription { get; set; }

        [Required(ErrorMessage = "Mood journal type id is required")]
        public string MoodJournalTypeId { get; set; }

    }
}

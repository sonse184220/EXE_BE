using Repository.Models;

namespace Repository.Interfaces
{
    public interface IMoodJournalRepository
    {
        Task<int> CreateMoodJournalAsync(MoodJournal moodJournal);

        Task<int> UpdateMoodJournalAsync(MoodJournal moodJournal);

        Task<List<MoodJournal>> GetAllMoodJournalAsync();

        Task<MoodJournal> GetMoodJournalByIdAsync(string moodJournalId);


        Task<List<MoodJournal>> GetAllMoodJournalByProfileIdAsync(string profileId);
    }
}

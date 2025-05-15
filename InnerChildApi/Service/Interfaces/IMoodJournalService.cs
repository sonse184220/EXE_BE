using Repository.Models;

namespace Service.Interfaces
{
    public interface IMoodJournalService
    {
        Task<int> CreateMoodJournalAsync(MoodJournal moodJournal);

        Task<int> UpdateMoodJournalAsync(MoodJournal moodJournal);

        Task<List<MoodJournal>> GetAllMoodJournalAsync();

        Task<MoodJournal> GetMoodJournalByIdAsync(string moodJournalId);

        Task<List<MoodJournal>> GetAllMoodJournalByProfileIdAsync(string profileId);



        //mood journal type
        Task<int> CreateMoodJournalTypeAsync(MoodJournalType moodJournalType);

        Task<int> UpdateMoodJournalTypeAsync(MoodJournalType moodJournalType);

        Task<List<MoodJournalType>> GetAllMoodJournalTypeAsync();
        Task<MoodJournalType> GetMoodJournalTypeByIdAsync(string moodJournalTypeId);

    }
}

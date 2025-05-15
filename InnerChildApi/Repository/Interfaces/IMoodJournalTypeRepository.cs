using Repository.Models;

namespace Repository.Interfaces
{
    public interface IMoodJournalTypeRepository
    {
        Task<int> CreateMoodJournalTypeAsync(MoodJournalType moodJournalType);

        Task<int> UpdateMoodJournalTypeAsync(MoodJournalType moodJournalType);

        Task<List<MoodJournalType>> GetAllMoodJournalTypeAsync();

        Task<MoodJournalType> GetMoodJournalTypeByIdAsync(string moodJournalTypeId);

    }
}

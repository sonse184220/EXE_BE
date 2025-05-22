using Repository.Base;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Repositories
{
    public interface IMoodJournalTypeRepository
    {
        Task<int> CreateMoodJournalTypeAsync(MoodJournalType moodJournalType);

        Task<int> UpdateMoodJournalTypeAsync(MoodJournalType moodJournalType);

        Task<List<MoodJournalType>> GetAllMoodJournalTypeAsync();

        Task<MoodJournalType> GetMoodJournalTypeByIdAsync(string moodJournalTypeId);

    }
    public class MoodJournalTypeRepository : GenericRepository<MoodJournalType>, IMoodJournalTypeRepository
    {
        public MoodJournalTypeRepository() : base()
        {

        }
        public MoodJournalTypeRepository(InnerChildExeContext context) : base(context)
        {
        }
        public async Task<int> CreateMoodJournalTypeAsync(MoodJournalType moodJournalType)
        {
            return await CreateAsync(moodJournalType);
        }
        public async Task<int> UpdateMoodJournalTypeAsync(MoodJournalType moodJournalType)
        {
            return await UpdateAsync(moodJournalType);
        }
        public async Task<List<MoodJournalType>> GetAllMoodJournalTypeAsync()
        {
            return await GetAllAsync();
        }

        public async Task<MoodJournalType> GetMoodJournalTypeByIdAsync(string moodJournalTypeId)
        {
            return await GetByIdAsync(moodJournalTypeId);
        }
    }
}

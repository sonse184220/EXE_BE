using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Repositories
{
    public interface IMoodJournalRepository
    {
        Task<int> CreateMoodJournalAsync(MoodJournal moodJournal);

        Task<int> UpdateMoodJournalAsync(MoodJournal moodJournal);

        Task<List<MoodJournal>> GetAllMoodJournalAsync();

        Task<MoodJournal> GetMoodJournalByIdAsync(string moodJournalId);


        Task<List<MoodJournal>> GetAllMoodJournalByProfileIdAsync(string profileId);
    }
    public class MoodJournalRepository : GenericRepository<MoodJournal>, IMoodJournalRepository
    {
        public MoodJournalRepository() : base()
        {
        }
        public MoodJournalRepository(InnerChildExeContext context) : base(context)
        {
        }
        public async Task<int> CreateMoodJournalAsync(MoodJournal moodJournal)
        {
            return await CreateAsync(moodJournal);
        }
        public async Task<int> UpdateMoodJournalAsync(MoodJournal moodJournal)
        {
            return await UpdateAsync(moodJournal);
        }
        public async Task<List<MoodJournal>> GetAllMoodJournalAsync()
        {
            return await GetAllAsync();
        }

        public async Task<MoodJournal> GetMoodJournalByIdAsync(string moodJournalId)
        {
            return await GetByIdAsync(moodJournalId);
        }

        public async Task<List<MoodJournal>> GetAllMoodJournalByProfileIdAsync(string profileId)
        {
            return await _context.MoodJournals.Include(x => x.MoodJournalType).Where(x => x.ProfileId == profileId).ToListAsync();
        }
    }

}

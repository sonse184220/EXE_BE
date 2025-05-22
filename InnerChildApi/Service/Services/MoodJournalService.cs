using Repository.Models;
using Repository.Repositories;

namespace Service.Services
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
    public class MoodJournalService : IMoodJournalService
    {
        private readonly IMoodJournalRepository _moodJournalRepository;
        private readonly IMoodJournalTypeRepository _moodJournalTypeRepository;

        public MoodJournalService(IMoodJournalTypeRepository moodJournalTypeRepository, IMoodJournalRepository moodJournalRepository)
        {
            _moodJournalTypeRepository = moodJournalTypeRepository;
            _moodJournalRepository = moodJournalRepository;
        }

        public async Task<int> CreateMoodJournalAsync(MoodJournal moodJournal)
        {
            return await _moodJournalRepository.CreateMoodJournalAsync(moodJournal);
        }

        public async Task<int> CreateMoodJournalTypeAsync(MoodJournalType moodJournalType)
        {
            return await _moodJournalTypeRepository.CreateMoodJournalTypeAsync(moodJournalType);
        }

        public async Task<List<MoodJournal>> GetAllMoodJournalAsync()
        {
            return await _moodJournalRepository.GetAllMoodJournalAsync();
        }

        public async Task<List<MoodJournal>> GetAllMoodJournalByProfileIdAsync(string profileId)
        {
            return await _moodJournalRepository.GetAllMoodJournalByProfileIdAsync(profileId);
        }

        public async Task<List<MoodJournalType>> GetAllMoodJournalTypeAsync()
        {
            return await _moodJournalTypeRepository.GetAllMoodJournalTypeAsync();
        }

        public async Task<MoodJournal> GetMoodJournalByIdAsync(string moodJournalId)
        {
            return await _moodJournalRepository.GetMoodJournalByIdAsync(moodJournalId);
        }

        public async Task<MoodJournalType> GetMoodJournalTypeByIdAsync(string moodJournalTypeId)
        {
            return await _moodJournalTypeRepository.GetMoodJournalTypeByIdAsync(moodJournalTypeId);
        }

        public async Task<int> UpdateMoodJournalAsync(MoodJournal moodJournal)
        {
            return await _moodJournalRepository.UpdateMoodJournalAsync(moodJournal);
        }

        public async Task<int> UpdateMoodJournalTypeAsync(MoodJournalType moodJournalType)
        {
            return await _moodJournalTypeRepository.UpdateMoodJournalTypeAsync(moodJournalType);
        }
    }
}

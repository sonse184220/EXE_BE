using Repository.Models;
using Repository.Repositories;
namespace Service.Services
{
    public interface IAudioService
    {
        Task<List<AudioCategory>> GetAllAudioCategoryAsync();
        Task<int> CreateAudioCategoryAsync(AudioCategory audioCategory);
        Task<AudioCategory> GetAudioCategoryByIdAsync(string audioCategoryId);
        Task<int> UpdateAudioCategoryAsync(AudioCategory audioCategory);


        Task<List<Audio>> GetAllAudioAsync();
        Task<int> CreateAudioAsync(Audio audio);
        Task<Audio> GetAudioByIdAsync(string audioId);
        Task<int> UpdateAudioAsync(Audio audio);


        Task<List<SubAudioCategory>> GetAllSubAudioCategoryAsync();
        Task<int> CreateSubAudioCategoryAsync(SubAudioCategory subAudioCategory);
        Task<SubAudioCategory> GetSubAudioCategoryByIdAsync(string subAudioCategoryId);
        Task<int> UpdateSubAudioSubCategoryAsync(SubAudioCategory subAudioCategory);
    }
    public class AudioService : IAudioService
    {
        private readonly IAudioRepository _audioRepo;
        private readonly ISubAudioCategoryRepository _subAudioCategoryRepo;
        private readonly IAudioCategoryRepository _audioCategoryRepo;
        public AudioService(IAudioRepository audioRepo, ISubAudioCategoryRepository subAudioCategoryRepo, IAudioCategoryRepository audioCategoryRepo)
        {
            _subAudioCategoryRepo = subAudioCategoryRepo;
            _audioCategoryRepo = audioCategoryRepo;
            _audioRepo = audioRepo;
        }
        public async Task<int> CreateAudioAsync(Repository.Models.Audio audio)
        {
            return await _audioRepo.CreateAudioAsync(audio);
        }

        public async Task<int> CreateAudioCategoryAsync(AudioCategory audioCategory)
        {
            return await _audioCategoryRepo.CreateAudioCategoryAsync(audioCategory);
        }

        public async Task<int> CreateSubAudioCategoryAsync(SubAudioCategory subAudioCategory)
        {
            return await _subAudioCategoryRepo.CreateSubAudioCategoryAsync(subAudioCategory);
        }

        public async Task<List<Repository.Models.Audio>> GetAllAudioAsync()
        {
            return await _audioRepo.GetAllAudioAsync();
        }

        public async Task<List<AudioCategory>> GetAllAudioCategoryAsync()
        {
            return await _audioCategoryRepo.GetAllAudioCategoryAsync();
        }

        public async Task<List<SubAudioCategory>> GetAllSubAudioCategoryAsync()
        {
            return await _subAudioCategoryRepo.GetAllSubAudioCategoryAsync();

        }

        public async Task<Repository.Models.Audio> GetAudioByIdAsync(string audioId)
        {
            return await _audioRepo.GetAudioByIdAsync(audioId);
        }

        public async Task<AudioCategory> GetAudioCategoryByIdAsync(string audioCategoryId)
        {
            return await _audioCategoryRepo.GetAudioCategoryByIdAsync(audioCategoryId);

        }

        public async Task<SubAudioCategory> GetSubAudioCategoryByIdAsync(string subAudioCategoryId)
        {
            return await _subAudioCategoryRepo.GetSubAudioCategoryByIdAsync(subAudioCategoryId);
        }

        public async Task<int> UpdateAudioAsync(Repository.Models.Audio audio)
        {
            return await _audioRepo.UpdateAudioAsync(audio);
        }

        public async Task<int> UpdateAudioCategoryAsync(AudioCategory audioCategory)
        {
            return await _audioCategoryRepo.UpdateAudioCategoryAsync(audioCategory);

        }

        public async Task<int> UpdateSubAudioSubCategoryAsync(SubAudioCategory subAudioCategory)
        {
            return await _subAudioCategoryRepo.UpdateSubAudioSubCategoryAsync(subAudioCategory);

        }
    }
}

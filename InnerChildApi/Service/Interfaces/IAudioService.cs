using Repository.Models;

namespace Service.Interfaces
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
}

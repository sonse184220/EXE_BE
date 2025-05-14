using Repository.Models;

namespace Repository.Interfaces
{
    public interface IAudioCategoryRepository
    {
        Task<List<AudioCategory>> GetAllAudioCategoryAsync();
        Task<int> CreateAudioCategoryAsync(AudioCategory audioCategory);
        Task<AudioCategory> GetAudioCategoryByIdAsync(string audioCategoryId);
        Task<int> UpdateAudioCategoryAsync(AudioCategory audioCategory);

    }
}

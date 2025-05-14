using Repository.Models;

namespace Repository.Interfaces
{
    public interface ISubAudioCategoryRepository
    {

        Task<List<SubAudioCategory>> GetAllSubAudioCategoryAsync();
        Task<int> CreateSubAudioCategoryAsync(SubAudioCategory subAudioCategory);
        Task<SubAudioCategory> GetSubAudioCategoryByIdAsync(string subAudioCategoryId);
        Task<int> UpdateSubAudioSubCategoryAsync(SubAudioCategory subAudioCategory);
    }
}

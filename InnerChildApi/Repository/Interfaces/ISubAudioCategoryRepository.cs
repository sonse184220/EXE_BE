using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

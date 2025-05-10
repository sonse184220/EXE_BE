using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAudioRepository
    {
        Task<List<Audio>> GetAllAudioAsync();
        Task<int> CreateAudioAsync(Audio audio );
        Task<Audio> GetAudioByIdAsync(string audioId);
        Task<int> UpdateAudioAsync(Audio audio );
    }
}

using Repository.Base;
using Repository.Models;

namespace Repository.Repositories
{
    public interface IHelpRepository
    {
        Task<List<Help>> GetAllHelpAsync();
        Task<Help> GetHelpByIdAsync(string id);
        Task<int> CreateHelpAsync(Help help);
        Task<int> UpdateHelpAsync(Help help);

        Task<bool> DeleteHelpAsync(Help help);
    }
    public class HelpRepository : GenericRepository<Help>, IHelpRepository
    {
        public async Task<int> CreateHelpAsync(Help help)
        {
            return await CreateAsync(help);
        }

        public async Task<bool> DeleteHelpAsync(Help help)
        {
            return await RemoveAsync(help);
        }

        public async Task<List<Help>> GetAllHelpAsync()
        {
            return await GetAllAsync();
        }

        public async Task<Help> GetHelpByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<int> UpdateHelpAsync(Help help)
        {
            return await UpdateAsync(help);
        }
    }
}

using Repository.Base;
using Repository.Models;

namespace Repository.Repositories
{
    public interface IHelpCategoryRepository
    {
        Task<List<HelpCategory>> GetAllHelpCategoryAsync();
        Task<HelpCategory> GetHelpCategoryByIdAsync(string id);
        Task<int> CreateHelpCategoryAsync(HelpCategory helpCategory);
        Task<int> UpdateHelpCategoryAsync(HelpCategory helpCategory);

        Task<bool> DeleteHelpCategoryAsync(HelpCategory helpCategory);

    }
    public class HelpCategoryRepository : GenericRepository<HelpCategory>, IHelpCategoryRepository
    {
        public async Task<int> CreateHelpCategoryAsync(HelpCategory helpCategory)
        {
            return await CreateAsync(helpCategory);
        }

        public async Task<List<HelpCategory>> GetAllHelpCategoryAsync()
        {
            return await GetAllAsync();
        }

        public async Task<HelpCategory> GetHelpCategoryByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteHelpCategoryAsync(HelpCategory helpCategory)
        {
            var help = _context.Helps.Where(x => x.HelpCategoryId == helpCategory.HelpCategoryId);
            _context.Helps.RemoveRange(help);
            _context.HelpCategories.Remove(helpCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> UpdateHelpCategoryAsync(HelpCategory helpCategory)
        {
            return await UpdateAsync(helpCategory);
        }
    }
}

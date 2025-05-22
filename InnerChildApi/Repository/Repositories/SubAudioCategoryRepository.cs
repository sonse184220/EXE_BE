using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.DBContext;
using Repository.Models;

namespace Repository.Repositories
{
    public interface ISubAudioCategoryRepository
    {

        Task<List<SubAudioCategory>> GetAllSubAudioCategoryAsync();
        Task<int> CreateSubAudioCategoryAsync(SubAudioCategory subAudioCategory);
        Task<SubAudioCategory> GetSubAudioCategoryByIdAsync(string subAudioCategoryId);
        Task<int> UpdateSubAudioSubCategoryAsync(SubAudioCategory subAudioCategory);
    }
    public class SubAudioCategoryRepository : GenericRepository<SubAudioCategory>, ISubAudioCategoryRepository
    {
        public SubAudioCategoryRepository() : base()
        {
        }
        public SubAudioCategoryRepository(InnerChildExeContext context) : base(context)
        {


        }
        public async Task<int> CreateSubAudioCategoryAsync(SubAudioCategory subAudioCategory)
        {
            return await base.CreateAsync(subAudioCategory);
        }

        public async Task<List<SubAudioCategory>> GetAllSubAudioCategoryAsync()
        {
            return await _context.SubAudioCategories.Include(x => x.AudioCategory).ToListAsync();
        }


        public async Task<SubAudioCategory> GetSubAudioCategoryByIdAsync(string subAudioCategoryId)
        {
            return await _context.SubAudioCategories.Include(x => x.AudioCategory).FirstOrDefaultAsync(x => x.SubAudioCategoryId == subAudioCategoryId);
        }

        public async Task<int> UpdateSubAudioSubCategoryAsync(SubAudioCategory subAudioCategory)
        {
            return await base.UpdateAsync(subAudioCategory);
        }
    }
}

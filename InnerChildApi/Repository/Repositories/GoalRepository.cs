using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Models;

namespace Repository.Repositories
{
    public interface IGoalRepository
    {
        Task<List<Goal>> GetAllOwnGoalsAsync(string profileId);
        Task<Goal> GetGoalByIdAsync(string goalId, string profileId);
        Task<int> CreateGoalAsync(Goal goal);
        Task<int> UpdateGoalAsync(Goal goal);

        Task<bool> DeleteGoalAsync(Goal goal);

    }
    public class GoalRepository : GenericRepository<Goal>, IGoalRepository
    {
        public async Task<int> CreateGoalAsync(Goal goal)
        {
            return await CreateAsync(goal);
        }

        public async Task<bool> DeleteGoalAsync(Goal goal)
        {
            return await RemoveAsync(goal);
        }

        public async Task<List<Goal>> GetAllOwnGoalsAsync(string profileId)
        {
            return await _context.Goals.Where(x => x.ProfileId == profileId).ToListAsync();
        }

        public async Task<Goal> GetGoalByIdAsync(string goalId, string profileId)
        {
            return await _context.Goals.FirstOrDefaultAsync(x => x.GoalId == goalId && x.ProfileId == profileId);
        }

        public async Task<int> UpdateGoalAsync(Goal goal)
        {
            return await UpdateAsync(goal);
        }
    }
}

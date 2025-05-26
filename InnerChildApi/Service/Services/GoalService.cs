using Repository.Models;
using Repository.Repositories;

namespace Service.Services
{
    public interface IGoalService
    {
        Task<List<Goal>> GetAllOwnGoalsAsync(string profileId);
        Task<Goal> GetGoalByIdAsync(string goalId, string profileId);
        Task<int> CreateGoalAsync(Goal goal);
        Task<int> UpdateGoalAsync(Goal goal);

        Task<bool> DeleteGoalAsync(Goal goal);
    }
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;
        public GoalService(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public async Task<int> CreateGoalAsync(Goal goal)
        {
            return await _goalRepository.CreateGoalAsync(goal);
        }

        public async Task<bool> DeleteGoalAsync(Goal goal)
        {
            return await _goalRepository.DeleteGoalAsync(goal);
        }

        public async Task<List<Goal>> GetAllOwnGoalsAsync(string profileId)
        {
            return await _goalRepository.GetAllOwnGoalsAsync(profileId);
        }

        public async Task<Goal> GetGoalByIdAsync(string goalId, string profileId)
        {
            return await _goalRepository.GetGoalByIdAsync(goalId, profileId);
        }

        public async Task<int> UpdateGoalAsync(Goal goal)
        {
            return await _goalRepository.UpdateGoalAsync(goal);
        }
    }
}

using Repository.Models;
using Repository.Repositories;

namespace Service.Services
{
    public interface IHelpAndAnswerService
    {
        Task<List<HelpCategory>> GetAllHelpCategoryAsync();
        Task<HelpCategory> GetHelpCategoryByIdAsync(string id);
        Task<int> CreateHelpCategoryAsync(HelpCategory helpCategory);
        Task<int> UpdateHelpCategoryAsync(HelpCategory helpCategory);
        Task<bool> DeleteHelpCategoryAsync(HelpCategory helpCategory);





        Task<List<Help>> GetAllHelpAsync();
        Task<Help> GetHelpByIdAsync(string id);
        Task<int> CreateHelpAsync(Help help);
        Task<int> UpdateHelpAsync(Help help);
        Task<bool> DeleteHelpAsync(Help help);

    }
    public class HelpService : IHelpAndAnswerService
    {
        private readonly IHelpCategoryRepository _helpCategoryRepository;
        private readonly IHelpRepository _helpRepository;
        public HelpService(IHelpRepository helpRepository, IHelpCategoryRepository helpCategoryRepository)
        {
            _helpCategoryRepository = helpCategoryRepository;
            _helpRepository = helpRepository;
        }
        public async Task<int> CreateHelpAsync(Help help)
        {
            return await _helpRepository.CreateHelpAsync(help);
        }

        public async Task<int> CreateHelpCategoryAsync(HelpCategory helpCategory)
        {
            return await _helpCategoryRepository.CreateHelpCategoryAsync(helpCategory);

        }

        public async Task<bool> DeleteHelpAsync(Help help)
        {
            return await _helpRepository.DeleteHelpAsync(help);
        }

        public async Task<bool> DeleteHelpCategoryAsync(HelpCategory helpCategory)
        {
            return await _helpCategoryRepository.DeleteHelpCategoryAsync(helpCategory);
        }

        public async Task<List<Help>> GetAllHelpAsync()
        {
            return await _helpRepository.GetAllHelpAsync();
        }

        public async Task<List<HelpCategory>> GetAllHelpCategoryAsync()
        {
            return await _helpCategoryRepository.GetAllHelpCategoryAsync();
        }

        public async Task<Help> GetHelpByIdAsync(string id)
        {
            return await _helpRepository.GetHelpByIdAsync(id);
        }

        public async Task<HelpCategory> GetHelpCategoryByIdAsync(string id)
        {
            return await _helpCategoryRepository.GetHelpCategoryByIdAsync(id);

        }

        public async Task<int> UpdateHelpAsync(Help help)
        {
            return await _helpRepository.UpdateHelpAsync(help);
        }

        public async Task<int> UpdateHelpCategoryAsync(HelpCategory helpCategory)
        {
            return await _helpCategoryRepository.UpdateHelpCategoryAsync(helpCategory);

        }
    }
}

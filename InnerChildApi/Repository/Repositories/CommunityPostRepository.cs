using Repository.Base;
using Repository.DBContext;
using Repository.Interfaces;
using Repository.Models;

namespace Repository.Repositories
{
    public class CommunityPostRepository : GenericRepository<CommunityPost>, ICommunityPostRepository
    {
        public CommunityPostRepository() : base()
        {

        }
        public CommunityPostRepository(InnerChildExeContext context) : base(context)
        {
        }
        public async Task<CommunityPost> GetCommunityPostByIdAsync(string communityPostId)
        {
            return await GetByIdAsync(communityPostId);
        }
        public async Task<IEnumerable<CommunityPost>> GetAllCommunityPostsAsync()
        {
            return await GetAllAsync();
        }
        public async Task<int> CreateCommunityPostAsync(CommunityPost communityPost)
        {
            return await CreateAsync(communityPost);
        }
        public async Task<int> UpdateCommunityPostAsync(CommunityPost communityPost)
        {
            return await UpdateAsync(communityPost);
        }
    }
}

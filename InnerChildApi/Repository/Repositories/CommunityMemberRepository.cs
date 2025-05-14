using Repository.Base;
using Repository.DBContext;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    internal class CommunityMemberRepository : GenericRepository<CommunityMember>, ICommunityMemberRepository
    {
        public CommunityMemberRepository() : base()
        {

        }
        public CommunityMemberRepository(InnerChildExeContext context) : base(context)
        {
        }
        public async Task<CommunityMember> GetCommunityMembersByIdAsync(string communityMemberId)
        {
            return await base.GetByIdAsync(communityMemberId);
        }
        public async Task<IEnumerable<CommunityMember>> GetAllCommunityMemberAsync()
        {
            return await base.GetAllAsync();
        }
        public async Task<int> CreateCommunityMemberAsync(CommunityMember communityMember)
        {
            return await base.CreateAsync(communityMember);
        }
        public async Task<int> UpdateUserCommunityMemberAsync(CommunityMember communityMember)
        {
            return await base.UpdateAsync(communityMember);
        }
    }
    
}

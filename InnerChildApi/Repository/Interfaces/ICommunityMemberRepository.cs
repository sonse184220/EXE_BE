using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ICommunityMemberRepository
    {
        Task<CommunityMember> GetCommunityMembersByIdAsync(string communityMemberId);


        Task<IEnumerable<CommunityMember>> GetAllCommunityMemberAsync();


        Task<int> CreateCommunityMemberAsync(CommunityMember communityMember);


        Task<int> UpdateUserCommunityMemberAsync(CommunityMember communityMember);
    }
}

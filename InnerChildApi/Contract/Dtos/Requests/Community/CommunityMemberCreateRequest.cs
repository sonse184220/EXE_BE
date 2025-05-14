using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Contract.Dtos.Requests.Community
{
    public class CommunityMemberCreateRequest
    {
        public string CommunityGroupId { get; set; }
    }
}

using Contract.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Requests.Community
{
    public class CommunityUpdateRequest
    {
        public string? CommunityName { get; set; }

        public string? CommunityDescription { get; set; }

        public CommunityEnum? CommunityStatus { get; set; }
    }
}

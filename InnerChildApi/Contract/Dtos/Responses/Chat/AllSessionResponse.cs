using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Responses.Chat
{
    public class AllSessionResponse
    {
        
        public string AichatSessionId { get; set; }

        public string AichatSessionTitle { get; set; }

        public DateTime? AichatSessionCreatedAt { get; set; }

        public string ProfileId { get; set; }
    }
}

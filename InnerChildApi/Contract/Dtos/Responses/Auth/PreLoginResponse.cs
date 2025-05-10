using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Responses.Auth
{
    public class PreLoginResponse
    {
        public string UserId { get; set; }
        public string ProfileId { get; set; }
        public string ProfileStatus { get; set; }
        public string ProfileToken { get; set; }
    }
}

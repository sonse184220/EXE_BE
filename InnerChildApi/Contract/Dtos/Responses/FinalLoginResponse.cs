using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Responses
{
    public class FinalLoginResponse
    {
        public string Token { get; set; }   
    }


    public class PreFinalLoginResponse
    {
        public string UserId { get; set; }
        public string ProfileId { get; set; }
    }
}

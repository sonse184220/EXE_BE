using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.HubModel
{
    public class ConnectionModel
    {
        public string UserId { get; set; }  
        public string ProfileId { get; set; }
        public string SessionId { get; set; }   
    }
}

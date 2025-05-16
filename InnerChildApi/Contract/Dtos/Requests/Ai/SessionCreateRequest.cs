using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Contract.Dtos.Requests.Ai
{
    public class SessionCreateRequest
    {
        public string? SessionTitle { get; set; }
    }
}

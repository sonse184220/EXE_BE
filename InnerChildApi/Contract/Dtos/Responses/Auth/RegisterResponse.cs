using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Responses.Auth
{
    public class RegisterResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}

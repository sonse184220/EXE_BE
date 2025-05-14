using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Contract.Dtos.Requests.Community
{
    public class CommunityPostCreateRequest
    {

        public string CommunityPostTitle { get; set; }

        public string CommunityPostContent { get; set; }

        public  IFormFile? CommunityPostImageFile { get; set; }
        public string CommunityGroupId { get; set; }
        
    }
}

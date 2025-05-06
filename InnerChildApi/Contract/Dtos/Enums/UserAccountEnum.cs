using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Contract.Dtos.Enums
{
    public enum UserAccountEnum
    {
        Active,
        Inactive,
    }
    public enum UserGenderEnum
    {
        Male ,
        Female, 
        Other
    }
    public enum JwtTypeEnum
    {
        PreLogin,
        FinalLogin,
        EmailConfirm,
        Other
    }
}

namespace Contract.Common.Enums
{
    public enum UserAccountEnum
    {
        Active,
        Inactive,
    }
    public enum UserGenderEnum
    {
        Male,
        Female,
        Other
    }
    public enum JwtTypeEnum
    {
        PreLogin,
        FinalLogin,
        EmailConfirm,
        ForgotPassword,
        ResetPassword,
        Other
    }
}

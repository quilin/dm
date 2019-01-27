namespace DM.Services.Authentication.Dto
{
    public enum AuthenticationError
    {
        NoError = 0,
        WrongLogin = 1,
        WrongPassword = 2,
        Banned = 3,
        Inactive = 4,
        Removed = 5,
        SessionExpired = 6,
        Forbidden = 7
    }
}
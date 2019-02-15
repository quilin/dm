using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Dto
{
    public interface IIdentity
    {
        AuthenticatedUser User { get; }
        Session Session { get; }
        UserSettings Settings { get; }

        AuthenticationError Error { get; }
        string Token { get; }
    }
}
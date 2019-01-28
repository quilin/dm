using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Extensions
{
    public static class UserExtensions
    {
        public static bool IsGuest(this IUser user) => user.Role == UserRole.Guest;
    }
}
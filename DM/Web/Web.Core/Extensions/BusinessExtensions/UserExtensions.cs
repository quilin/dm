using System;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace Web.Core.Extensions.BusinessExtensions
{
    public static class UserExtensions
    {
        public static bool IsOnline(this IUser user)
        {
            return user.LastVisitDate.HasValue && (DateTime.UtcNow - user.LastVisitDate.Value).TotalMinutes < 5;
        }
    }
}
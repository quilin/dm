using System;
using System.Linq;
using System.Net;
using System.Text;
using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;

namespace DM.Services.Common.Implementation
{
    /// <summary>
    /// Specific exception when user tries to perform unauthorized action
    /// </summary>
    public class IntentionManagerException : HttpException
    {
        public IntentionManagerException(IUser user, Enum intention, object target)
            : base(HttpStatusCode.Forbidden, GenerateMessage(user, intention, target))
        {
        }

        private static string GenerateMessage(IUser user, Enum intention, object target)
        {
            var userDisplayName = user == AuthenticatedUser.Guest
                ? user.Role.ToString()
                : $"{user.Login} ({string.Join(", ", Enum.GetValues(typeof(UserRole)).Cast<UserRole>().Where(u => user.Role.HasFlag(u)))})";
            var result = new StringBuilder($"User {userDisplayName} is not allowed to perform {intention} action");
            if (target == null)
            {
                return result.ToString();
            }

            var subjectType = target.GetType();
            var subjectName = subjectType.Name;
            result.Append($" with object {subjectName}");
            var subjectId = subjectType.GetProperty("Id")?.GetValue(target);
            if (subjectId != null)
            {
                result.Append($" with id {subjectId}");
            }

            return result.ToString();
        }
    }
}
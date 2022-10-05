using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.BusinessProcesses.Common;

/// <inheritdoc />
internal class AccessPolicyConverter : IAccessPolicyConverter
{
    /// <inheritdoc />
    public ForumAccessPolicy Convert(UserRole role)
    {
        if (role == UserRole.Guest)
        {
            return ForumAccessPolicy.Guest;
        }

        var result = ForumAccessPolicy.Guest | ForumAccessPolicy.Player;
        if (role.HasFlag(UserRole.Administrator))
        {
            result = result | ForumAccessPolicy.Administrator;
        }

        if (role.HasFlag(UserRole.SeniorModerator))
        {
            result = result | ForumAccessPolicy.SeniorModerator;
        }

        if (role.HasFlag(UserRole.RegularModerator))
        {
            result = result | ForumAccessPolicy.RegularModerator;
        }

        if (role.HasFlag(UserRole.NannyModerator))
        {
            result = result | ForumAccessPolicy.NannyModerator;
        }

        return result;
    }
}
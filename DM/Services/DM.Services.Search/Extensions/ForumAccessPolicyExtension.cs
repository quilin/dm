using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Search.Extensions;

/// <summary>
/// Extensions for forum entities indexing
/// </summary>
public static class ForumAccessPolicyExtension
{
    /// <summary>
    /// Get list of atomic roles that are authorized to perform forum policy action
    /// </summary>
    /// <param name="forumAccessPolicy">Forum access policy</param>
    /// <returns>List of user rols</returns>
    public static IEnumerable<UserRole> GetAuthorizedRoles(this ForumAccessPolicy forumAccessPolicy)
    {
        if (forumAccessPolicy.HasFlag(ForumAccessPolicy.Guest))
        {
            yield return UserRole.Guest;
            yield return UserRole.Player;
        }

        if (forumAccessPolicy.HasFlag(ForumAccessPolicy.NannyModerator))
        {
            yield return UserRole.NannyModerator;
        }

        if (forumAccessPolicy.HasFlag(ForumAccessPolicy.ForumModerator))
        {
            yield return UserRole.RegularModerator;
        }

        if (forumAccessPolicy.HasFlag(ForumAccessPolicy.SeniorModerator))
        {
            yield return UserRole.SeniorModerator;
        }

        if (forumAccessPolicy.HasFlag(ForumAccessPolicy.Administrator))
        {
            yield return UserRole.Administrator;
        }
    }
}
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.BusinessProcesses.Common;

/// <summary>
/// User role to forum policy converter
/// </summary>
internal interface IAccessPolicyConverter
{
    /// <summary>
    /// Converts given user role into available composite forum access policy
    /// </summary>
    /// <param name="role">User role</param>
    /// <returns>Forum access policy</returns>
    ForumAccessPolicy Convert(UserRole role);
}
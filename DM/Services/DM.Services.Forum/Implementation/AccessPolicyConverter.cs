using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.Implementation
{
    /// <inheritdoc />
    public class AccessPolicyConverter : IAccessPolicyConverter
    {
        /// <inheritdoc />
        public ForumAccessPolicy Convert(UserRole role)
        {
            var result = ForumAccessPolicy.Guest;
            if (role != UserRole.Guest)
            {
                result = result | ForumAccessPolicy.Player;
            }

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
                result = result | ForumAccessPolicy.NurseModerator;
            }

            return result;
        }
    }
}
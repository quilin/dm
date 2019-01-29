using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Forum.Implementation
{
    public class AccessPolicyConverter : IAccessPolicyConverter
    {
        public ForumAccessPolicy Convert(UserRole role)
        {
            var result = ForumAccessPolicy.Everyone;
            if (role != UserRole.Guest)
            {
                result = result | ForumAccessPolicy.Players;
            }
            if (role.HasFlag(UserRole.Administrator))
            {
                result = result | ForumAccessPolicy.Administrators;
            }
            if (role.HasFlag(UserRole.SeniorModerator))
            {
                result = result | ForumAccessPolicy.SeniorModerators;
            }
            if (role.HasFlag(UserRole.RegularModerator))
            {
                result = result | ForumAccessPolicy.RegularModerators;
            }
            if (role.HasFlag(UserRole.NurseModerator))
            {
                result = result | ForumAccessPolicy.NannyModerators;
            }
            return result;
        }
    }
}
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.Implementation
{
    public interface IAccessPolicyConverter
    {
        ForumAccessPolicy Convert(UserRole role);
    }
}
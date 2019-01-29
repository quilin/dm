using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Forum.Implementation
{
    public interface IAccessPolicyConverter
    {
        ForumAccessPolicy Convert(UserRole role);
    }
}
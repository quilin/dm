using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Implementation;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.Implementation;

namespace DM.Services.Forum.Authorization
{
    public class ForumIntentionResolver : IIntentionResolver<ForumIntention, Dto.Forum>
    {
        private readonly IAccessPolicyConverter accessPolicyConverter;

        public ForumIntentionResolver(
            IAccessPolicyConverter accessPolicyConverter)
        {
            this.accessPolicyConverter = accessPolicyConverter;
        }
        
        public Task<bool> IsAllowed(AuthenticatedUser user, ForumIntention intention, Dto.Forum target)
        {
            switch (intention)
            {
                case ForumIntention.CreateTopic when user.IsAuthenticated:
                    var userPolicy = accessPolicyConverter.Convert(user.Role);
                    return Task.FromResult((target.CreateTopicPolicy & userPolicy) != ForumAccessPolicy.NoOne);
                default:
                    return Task.FromResult(false);
            }
        }
    }
}
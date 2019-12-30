using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization
{
    /// <inheritdoc />
    public class AttributeSchemaIntentionResolver : IIntentionResolver<AttributeSchemaIntention, AttributeSchema>
    {
        /// <inheritdoc />
        public bool IsAllowed(AuthenticatedUser user, AttributeSchemaIntention intention, AttributeSchema target)
        {
            switch (intention)
            {
                case AttributeSchemaIntention.Edit:
                case AttributeSchemaIntention.Delete:
                    return target.UserId == user.UserId;
                default:
                    return false;
            }
        }
    }
}
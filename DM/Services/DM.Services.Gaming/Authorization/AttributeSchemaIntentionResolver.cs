using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.Authorization;

/// <inheritdoc />
internal class AttributeSchemaIntentionResolver : IIntentionResolver<AttributeSchemaIntention, AttributeSchema>
{
    /// <inheritdoc />
    public bool IsAllowed(AuthenticatedUser user, AttributeSchemaIntention intention, AttributeSchema target) =>
        intention switch
        {
            AttributeSchemaIntention.Edit => target.Author.UserId == user.UserId,
            AttributeSchemaIntention.Delete => target.Author.UserId == user.UserId,
            _ => false
        };
}
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.Authorization;

/// <inheritdoc />
internal class CommentIntentionResolver : IIntentionResolver<CommentIntention, Comment>
{
    /// <inheritdoc />
    public bool IsAllowed(AuthenticatedUser user, CommentIntention intention, Comment target) => intention switch
    {
        CommentIntention.Edit when user.IsAuthenticated => user.Role.HasFlag(UserRole.Administrator) ||
                                                           target.Author.UserId == user.UserId,
        CommentIntention.Delete when user.IsAuthenticated => user.Role.HasFlag(UserRole.Administrator) ||
                                                             target.Author.UserId == user.UserId,
        CommentIntention.Like when user.IsAuthenticated => target.Author.UserId != user.UserId,
        _ => false
    };
}
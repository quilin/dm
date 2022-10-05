using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization;

/// <inheritdoc />
internal class PostIntentionResolver :
    IIntentionResolver<PostIntention, Post>,
    IIntentionResolver<PostIntention, (Post, RoomToUpdate)>
{
    /// <inheritdoc />
    public bool IsAllowed(AuthenticatedUser user, PostIntention intention, Post target)
    {
        return intention switch
        {
            PostIntention.Delete => target.Author.UserId == user.UserId,
            _ => false
        };
    }

    /// <inheritdoc />
    public bool IsAllowed(AuthenticatedUser user, PostIntention intention, (Post, RoomToUpdate) target)
    {
        var (post, room) = target;
        return intention switch
        {
            PostIntention.EditText => post.Author.UserId == user.UserId ||
                                      room.Game.Participation(user.UserId).HasFlag(GameParticipation.Authority) &&
                                      (post.Character == null || post.Character.IsNpc ||
                                       post.Character.AccessPolicy.HasFlag(CharacterAccessPolicy.PostEditAllowed)),
            PostIntention.EditCharacter => post.Author.UserId == user.UserId ||
                                           room.Game.Participation(user.UserId).HasFlag(GameParticipation.Authority) &&
                                           (post.Character == null || post.Character.IsNpc),
            PostIntention.EditMasterMessage => room.Game.Participation(user.UserId)
                .HasFlag(GameParticipation.Authority) && (post.Character == null || post.Character.IsNpc),
            _ => false
        };
    }
}
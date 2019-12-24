using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization
{
    /// <inheritdoc />
    public class PostIntentionResolver :
        IIntentionResolver<PostIntention, Post>,
        IIntentionResolver<PostIntention, Post, UpdatePost>
    {
        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, PostIntention intention, Post target)
        {
            switch (intention)
            {
                case PostIntention.Delete:
                    return Task.FromResult(target.Author.UserId == user.UserId);
                default:
                    return Task.FromResult(false);
            }
        }

        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, PostIntention intention, (Post, UpdatePost) target)
        {
            var (post, updatePost) = target;
            
            switch (intention)
            {
                case PostIntention.Edit:
                    return Task.FromResult(post.Author.UserId == user.UserId ||
                        subTarget.Game.Participation(user.UserId).HasFlag(GameParticipation.Authority) &&
                        target.Character.AccessPolicy.HasFlag(CharacterAccessPolicy.PostEditAllowed));
                case PostIntention.EditCharacter:
                    return Task.FromResult(target.Author.UserId == user.UserId);
                default:
                    return Task.FromResult(false);
            }
        }
    }
}
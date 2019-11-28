using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization
{
    /// <inheritdoc cref="IIntentionResolver" />
    public class GameIntentionResolver :
        IIntentionResolver<GameIntention>,
        IIntentionResolver<GameIntention, Game>
    {
        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, GameIntention intention)
        {
            switch (intention)
            {
                case GameIntention.Create when user.IsAuthenticated:
                    return Task.FromResult(true);

                case GameIntention.Subscribe when user.IsAuthenticated:
                    return Task.FromResult(true);

                default:
                    return Task.FromResult(false);
            }
        }

        private static readonly IEnumerable<GameStatus> HiddenStates = new HashSet<GameStatus>
        {
            GameStatus.Draft,
            GameStatus.RequiresModeration,
            GameStatus.Moderation
        };

        /// <inheritdoc />
        public Task<bool> IsAllowed(AuthenticatedUser user, GameIntention intention, Game target)
        {
            if (intention != GameIntention.Read && intention != GameIntention.ReadComments && !user.IsAuthenticated)
            {
                return Task.FromResult(false);
            }

            var userIsHighAuthority = user.Role.HasFlag(UserRole.Administrator) ||
                user.Role.HasFlag(UserRole.SeniorModerator);
            var userIsNanny = userIsHighAuthority || user.Role.HasFlag(UserRole.NannyModerator);
            var participation = target.Participation(user.UserId);

            switch (intention)
            {
                case GameIntention.Read:
                    return Task.FromResult(userIsHighAuthority || participation.HasFlag(GameParticipation.Authority) ||
                        !HiddenStates.Contains(target.Status));

                case GameIntention.Edit when user.IsAuthenticated:
                case GameIntention.AdministrateRooms when user.IsAuthenticated:
                    return Task.FromResult(userIsHighAuthority || participation.HasFlag(GameParticipation.Authority));

                // only the master itself is allowed to remove the game
                case GameIntention.Delete when user.IsAuthenticated:
                    return Task.FromResult(userIsHighAuthority || user.UserId == target.Master.UserId);

                case GameIntention.SetStatusModeration when target.Status == GameStatus.RequiresModeration:
                    return Task.FromResult(userIsHighAuthority || userIsNanny);

                case GameIntention.SetStatusDraft when target.Status == GameStatus.Moderation:
                case GameIntention.SetStatusRequirement when target.Status == GameStatus.Moderation:
                    return Task.FromResult(userIsHighAuthority || participation.HasFlag(GameParticipation.Moderator));

                case GameIntention.SetStatusDraft when target.Status == GameStatus.Requirement:
                case GameIntention.SetStatusRequirement when target.Status == GameStatus.Draft:
                case GameIntention.SetStatusRequirement when target.Status == GameStatus.Active:
                case GameIntention.SetStatusActive when target.Status == GameStatus.Requirement:
                case GameIntention.SetStatusActive when target.Status == GameStatus.Frozen:
                case GameIntention.SetStatusActive when target.Status == GameStatus.Finished:
                case GameIntention.SetStatusActive when target.Status == GameStatus.Closed:
                case GameIntention.SetStatusFrozen when target.Status == GameStatus.Active:
                case GameIntention.SetStatusFinished when target.Status == GameStatus.Active:
                case GameIntention.SetStatusClosed when target.Status == GameStatus.Active:
                    return Task.FromResult(participation.HasFlag(GameParticipation.Authority));

                case GameIntention.ReadComments:
                    return Task.FromResult(target.CommentariesAccessMode != CommentariesAccessMode.Private ||
                        target.Participation(user.UserId) != GameParticipation.None);

                case GameIntention.CreateComment when user.IsAuthenticated:
                    return Task.FromResult(target.CommentariesAccessMode == CommentariesAccessMode.Public ||
                        target.Participation(user.UserId) != GameParticipation.None);

                case GameIntention.CreateCharacter when user.IsAuthenticated:
                    return Task.FromResult(
                        target.Status == GameStatus.Requirement || target.Status == GameStatus.Active);

                default:
                    return Task.FromResult(false);
            }
        }
    }
}
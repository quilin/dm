using System.Collections.Generic;
using System.Linq;
using DM.Services.Authentication.Dto;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Authorization;

/// <inheritdoc cref="IIntentionResolver" />
internal class GameIntentionResolver :
    IIntentionResolver<GameIntention>,
    IIntentionResolver<GameIntention, Game>
{
    /// <inheritdoc />
    public bool IsAllowed(AuthenticatedUser user, GameIntention intention) => intention switch
    {
        GameIntention.Create when user.IsAuthenticated => true,
        GameIntention.Subscribe when user.IsAuthenticated => true,
        GameIntention.SetStatusModeration when user.IsAuthenticated => user.Role.HasFlag(UserRole.Administrator) || user.Role.HasFlag(UserRole.SeniorModerator) ||
                                                                       user.Role.HasFlag(UserRole.NannyModerator),
        _ => false
    };

    private static readonly IEnumerable<GameStatus> HiddenStates = new HashSet<GameStatus>
    {
        GameStatus.Draft,
        GameStatus.RequiresModeration,
        GameStatus.Moderation
    };

    /// <inheritdoc />
    public bool IsAllowed(AuthenticatedUser user, GameIntention intention, Game target)
    {
        if (intention != GameIntention.Read && intention != GameIntention.ReadComments && !user.IsAuthenticated)
        {
            return false;
        }

        var userIsHighAuthority = user.Role.HasFlag(UserRole.Administrator) ||
                                  user.Role.HasFlag(UserRole.SeniorModerator);
        var userIsNanny = userIsHighAuthority || user.Role.HasFlag(UserRole.NannyModerator);
        var participation = target.Participation(user.UserId);

        return intention switch
        {
            GameIntention.Read => userIsHighAuthority || participation.HasFlag(GameParticipation.Authority) ||
                                  !HiddenStates.Contains(target.Status),
            GameIntention.Subscribe when user.IsAuthenticated => participation == GameParticipation.None,
            GameIntention.Unsubscribe when user.IsAuthenticated => participation.HasFlag(GameParticipation.Reader),

            GameIntention.Edit when user.IsAuthenticated => userIsHighAuthority ||
                                                            participation.HasFlag(GameParticipation.Authority),
            // only the master itself is allowed to remove the game
            GameIntention.Delete when user.IsAuthenticated => userIsHighAuthority ||
                                                              user.UserId == target.Master.UserId,

            GameIntention.SetStatusModeration when target.Status == GameStatus.RequiresModeration =>
                userIsHighAuthority || userIsNanny,
            GameIntention.SetStatusDraft when target.Status == GameStatus.Moderation =>
                userIsHighAuthority || participation.HasFlag(GameParticipation.Moderator),
            GameIntention.SetStatusRequirement when target.Status == GameStatus.Moderation =>
                userIsHighAuthority || participation.HasFlag(GameParticipation.Moderator),
            GameIntention.SetStatusDraft when target.Status == GameStatus.Requirement =>
                participation.HasFlag(GameParticipation.Authority),
            GameIntention.SetStatusRequirement when target.Status == GameStatus.Draft =>
                participation.HasFlag(GameParticipation.Authority),
            GameIntention.SetStatusRequirement when target.Status == GameStatus.Active =>
                participation.HasFlag(GameParticipation.Authority),
            GameIntention.SetStatusActive when target.Status == GameStatus.Requirement =>
                participation.HasFlag(GameParticipation.Authority),
            GameIntention.SetStatusActive when target.Status == GameStatus.Frozen =>
                participation.HasFlag(GameParticipation.Authority),
            GameIntention.SetStatusActive when target.Status == GameStatus.Finished =>
                participation.HasFlag(GameParticipation.Authority),
            GameIntention.SetStatusActive when target.Status == GameStatus.Closed =>
                participation.HasFlag(GameParticipation.Authority),
            GameIntention.SetStatusFrozen when target.Status == GameStatus.Active =>
                participation.HasFlag(GameParticipation.Authority),
            GameIntention.SetStatusFinished when target.Status == GameStatus.Active =>
                participation.HasFlag(GameParticipation.Authority),
            GameIntention.SetStatusClosed when target.Status == GameStatus.Active =>
                participation.HasFlag(GameParticipation.Authority),
            GameIntention.ReadComments => 
                target.CommentariesAccessMode != CommentariesAccessMode.Private ||
                target.Participation(user.UserId) != GameParticipation.None,
            GameIntention.CreateComment when user.IsAuthenticated =>
                target.CommentariesAccessMode == CommentariesAccessMode.Public ||
                target.Participation(user.UserId) != GameParticipation.None,
            GameIntention.CreateCharacter when user.IsAuthenticated =>
                target.Status == GameStatus.Requirement || target.Status == GameStatus.Active,
            _ => false
        };
    }
}
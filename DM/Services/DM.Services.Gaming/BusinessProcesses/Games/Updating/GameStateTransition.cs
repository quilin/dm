using System.Collections.Generic;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating
{
    /// <inheritdoc />
    public class GameStateTransition : IGameStateTransition
    {
        private readonly IIdentityProvider identityProvider;

        /// <inheritdoc />
        public GameStateTransition(
            IIdentityProvider identityProvider)
        {
            this.identityProvider = identityProvider;
        }

        /// <inheritdoc />
        public (bool success, bool? assignNanny) TryChange(GameExtended game, GameStatus targetStatus)
        {
            var authority = GetAuthority(game, identityProvider.Current.User);
            if (!TransitionRules.TryGetValue((game.Status, targetStatus), out var neededAuthority) ||
                (neededAuthority & authority) == default)
            {
                return (false, null);
            }

            switch (game.Status)
            {
                case GameStatus.RequiresModeration when targetStatus == GameStatus.RequiresModeration:
                    return (true, true);
                case GameStatus.Moderation:
                    return (true, false);
                default:
                    return (true, null);
            }
        }

        private static readonly IDictionary<(GameStatus from, GameStatus to), GameAuthority> TransitionRules =
            new Dictionary<(GameStatus, GameStatus), GameAuthority>
            {
                [(GameStatus.RequiresModeration, GameStatus.Moderation)] = GameAuthority.Nanny,

                [(GameStatus.Moderation, GameStatus.Draft)] = GameAuthority.Moderator,
                [(GameStatus.Moderation, GameStatus.Requirement)] = GameAuthority.Moderator,

                [(GameStatus.Draft, GameStatus.Requirement)] = GameAuthority.Owner,

                [(GameStatus.Requirement, GameStatus.Draft)] = GameAuthority.Owner,
                [(GameStatus.Requirement, GameStatus.Active)] = GameAuthority.Owner,
                
                [(GameStatus.Active, GameStatus.Requirement)] = GameAuthority.Owner,
                [(GameStatus.Active, GameStatus.Finished)] = GameAuthority.Owner,
                [(GameStatus.Active, GameStatus.Frozen)] = GameAuthority.Owner | GameAuthority.HighAuthority,
                [(GameStatus.Active, GameStatus.Closed)] = GameAuthority.Owner | GameAuthority.HighAuthority,
                
                [(GameStatus.Finished, GameStatus.Active)] = GameAuthority.Owner,
                
                [(GameStatus.Frozen, GameStatus.Active)] = GameAuthority.Owner,
                [(GameStatus.Frozen, GameStatus.Closed)] = GameAuthority.Owner | GameAuthority.HighAuthority,
                
                [(GameStatus.Closed, GameStatus.Active)] = GameAuthority.Owner | GameAuthority.HighAuthority
            };

        private static GameAuthority GetAuthority(GameExtended game, IUser user)
        {
            var result = GameAuthority.None;
            if (game.Master.UserId == user.UserId || game.Assistant?.UserId == user.UserId)
            {
                result |= GameAuthority.Owner;
            }

            if (game.Nanny?.UserId == user.UserId)
            {
                result |= GameAuthority.Moderator;
            }

            if (user.Role.HasFlag(UserRole.Administrator) || user.Role.HasFlag(UserRole.SeniorModerator))
            {
                result |= GameAuthority.HighAuthority | GameAuthority.Nanny;
            }

            if (user.Role.HasFlag(UserRole.NannyModerator))
            {
                result |= GameAuthority.Nanny;
            }

            return result;
        }
    }
}
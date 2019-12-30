using AutoMapper;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Gaming.Dto;
using DM.Services.Gaming.Dto.Output;
using ServiceGame = DM.Services.Gaming.Dto.Output.Game;

namespace DM.Web.API.Dto.Games
{
    /// <summary>
    /// Resolver for current user participation
    /// </summary>
    public class GameParticipationResolver :
        IValueResolver<ServiceGame, Game, GameParticipation>,
        IValueResolver<GameExtended, Game, GameParticipation>
    {
        private readonly IIdentity identity;

        /// <inheritdoc />
        public GameParticipationResolver(
            IIdentityProvider identityProvider)
        {
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public GameParticipation Resolve(
            ServiceGame source, Game destination, GameParticipation destMember, ResolutionContext context) =>
            source.Participation(identity.User.UserId);

        /// <inheritdoc />
        public GameParticipation Resolve(
            GameExtended source, Game destination, GameParticipation destMember, ResolutionContext context) =>
            source.Participation(identity.User.UserId);
    }
}
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
    public class GameParticipationResolver : IValueResolver<ServiceGame, Game, bool>,
        IValueResolver<GameExtended, Game, bool>
    {
        private readonly IIdentity identity;

        /// <inheritdoc />
        public GameParticipationResolver(
            IIdentityProvider identityProvider)
        {
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public bool Resolve(ServiceGame source, Game destination, bool destMember, ResolutionContext context) =>
            source.UserParticipates(identity.User.UserId);

        /// <inheritdoc />
        public bool Resolve(GameExtended source, Game destination, bool destMember, ResolutionContext context) =>
            source.UserParticipates(identity.User.UserId);
    }
}
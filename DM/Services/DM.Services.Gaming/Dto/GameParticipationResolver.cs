using System.Linq;
using AutoMapper;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games;

namespace DM.Services.Gaming.Dto
{
    /// <summary>
    /// Resolver for user participation in game
    /// </summary>
    public class GameParticipationResolver : IValueResolver<Game, Output.Game, bool>
    {
        private readonly IIdentity identity;

        /// <inheritdoc />
        public GameParticipationResolver(
            IIdentityProvider identityProvider)
        {
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public bool Resolve(Game source, Output.Game destination, bool destMember, ResolutionContext context)
        {
            var userId = identity.User.UserId;

            return source.MasterId == userId || source.AssistantId == userId || source.NannyId == userId ||
                   source.Characters.Any(c => c.UserId == userId && !c.IsRemoved && c.Status == CharacterStatus.Active);
        }
    }
}
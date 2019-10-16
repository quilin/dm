using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Gaming.BusinessProcesses.Games.AssistantAssignment
{
    /// <inheritdoc />
    public class AssignmentService : IAssignmentService
    {
        private readonly IIdentityProvider identityProvider;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IAssignmentRepository repository;

        /// <inheritdoc />
        public AssignmentService(
            IIdentityProvider identityProvider,
            IUpdateBuilderFactory updateBuilderFactory,
            IAssignmentRepository repository)
        {
            this.identityProvider = identityProvider;
            this.updateBuilderFactory = updateBuilderFactory;
            this.repository = repository;
        }

        /// <inheritdoc />
        public Task AcceptAssignment(Guid tokenId) => Assign(tokenId, true);

        /// <inheritdoc />
        public Task RejectAssignment(Guid tokenId) => Assign(tokenId, false);

        private async Task Assign(Guid tokenId, bool accept)
        {
            var userId = identityProvider.Current.User.UserId;
            var gameId = await repository.FindGameToAssign(tokenId, userId);
            if (!gameId.HasValue)
            {
                throw new HttpException(HttpStatusCode.Gone,
                    "Activation token is invalid! Address the technical support for further assistance");
            }

            var updateToken = updateBuilderFactory.Create<Token>(tokenId).Field(t => t.IsRemoved, true);
            var updateGame = updateBuilderFactory.Create<Game>(gameId.Value);
            if (accept)
            {
                updateGame.Field(g => g.AssistantId, userId);
            }

            await repository.AssignAssistant(updateGame, updateToken);
        }

        /// <inheritdoc />
        public async Task CancelAssignments(Guid gameId)
        {
            var updates = (await repository.FindAssignments(gameId))
                .Select(id => updateBuilderFactory.Create<Token>(id).Field(t => t.IsRemoved, true));
            await repository.Invalidate(updates);
        }
    }
}
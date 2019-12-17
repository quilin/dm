using System.Net;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using DM.Services.Gaming.Dto.Input;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Deleting
{
    /// <inheritdoc />
    public class BlacklistDeletingService : IBlacklistDeletingService
    {
        private readonly IValidator<OperateBlacklistLink> validator;
        private readonly IUserRepository userRepository;
        private readonly IGameReadingService gameReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IBlacklistDeletingRepository repository;
        private readonly IInvokedEventPublisher invokedEventPublisher;

        /// <inheritdoc />
        public BlacklistDeletingService(
            IValidator<OperateBlacklistLink> validator,
            IUserRepository userRepository,
            IGameReadingService gameReadingService,
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            IBlacklistDeletingRepository repository,
            IInvokedEventPublisher invokedEventPublisher)
        {
            this.validator = validator;
            this.userRepository = userRepository;
            this.gameReadingService = gameReadingService;
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.repository = repository;
            this.invokedEventPublisher = invokedEventPublisher;
        }
        
        /// <inheritdoc />
        public async Task Delete(OperateBlacklistLink operateBlacklistLink)
        {
            await validator.ValidateAndThrowAsync(operateBlacklistLink);
            var game = await gameReadingService.GetGame(operateBlacklistLink.GameId);
            await intentionManager.ThrowIfForbidden(GameIntention.Edit, game);

            var (_, userId) = await userRepository.FindUserId(operateBlacklistLink.Login);
            if (!game.BlacklistedUsers.TryGetValue(userId, out var linkId))
            {
                throw new HttpException(HttpStatusCode.Conflict, "User is not blacklisted");
            }

            var updateBuilder = updateBuilderFactory.Create<BlackListLink>(linkId).Delete();
            await repository.Delete(updateBuilder);
            await invokedEventPublisher.Publish(EventType.ChangedGame, game.Id);
        }
    }
}
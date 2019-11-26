using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.AssistantAssignment;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;
using Game = DM.Services.DataAccess.BusinessObjects.Games.Game;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating
{
    /// <inheritdoc />
    public class GameUpdatingService : IGameUpdatingService
    {
        private readonly IValidator<UpdateGame> validator;
        private readonly IIntentionManager intentionManager;
        private readonly IGameReadingService gameReadingService;
        private readonly IAssignmentService assignmentService;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IUserRepository userRepository;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IGameUpdatingRepository updatingRepository;
        private readonly IGameIntentionConverter intentionConverter;
        private readonly IInvokedEventPublisher publisher;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public GameUpdatingService(
            IValidator<UpdateGame> validator,
            IIntentionManager intentionManager,
            IGameReadingService gameReadingService,
            IAssignmentService assignmentService,
            IUpdateBuilderFactory updateBuilderFactory,
            IUserRepository userRepository,
            IDateTimeProvider dateTimeProvider,
            IGameUpdatingRepository updatingRepository,
            IGameIntentionConverter intentionConverter,
            IInvokedEventPublisher publisher,
            IIdentityProvider identityProvider)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.gameReadingService = gameReadingService;
            this.assignmentService = assignmentService;
            this.updateBuilderFactory = updateBuilderFactory;
            this.userRepository = userRepository;
            this.dateTimeProvider = dateTimeProvider;
            this.updatingRepository = updatingRepository;
            this.intentionConverter = intentionConverter;
            this.publisher = publisher;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<GameExtended> Update(UpdateGame updateGame)
        {
            await validator.ValidateAndThrowAsync(updateGame);
            var game = await gameReadingService.GetGameDetails(updateGame.GameId);
            await intentionManager.ThrowIfForbidden(GameIntention.Edit, game);

            var changes = updateBuilderFactory.Create<Game>(game.Id)
                .MaybeField(c => c.Title, updateGame.Title)
                .MaybeField(c => c.SettingName, updateGame.SettingName)
                .MaybeField(c => c.SystemName, updateGame.SystemName)
                .MaybeField(c => c.Info, updateGame.Info)
                .MaybeField(c => c.HideTemper, updateGame.HideTemper)
                .MaybeField(c => c.HideStory, updateGame.HideStory)
                .MaybeField(c => c.HideSkills, updateGame.HideSkills)
                .MaybeField(c => c.HideInventory, updateGame.HideInventory)
                .MaybeField(c => c.HideDiceResult, updateGame.HideDiceResult)
                .MaybeField(c => c.ShowPrivateMessages, updateGame.ShowPrivateMessages)
                .MaybeField(c => c.CommentariesAccessMode, updateGame.CommentariesAccessMode)
                .MaybeField(c => c.DisableAlignment, updateGame.DisableAlignment);

            var oldAssistant = game.Assistant ?? game.PendingAssistant;
            if (updateGame.AssistantLogin != default &&
                !updateGame.AssistantLogin.Equals(oldAssistant?.Login, StringComparison.InvariantCultureIgnoreCase))
            {
                var (assistantExists, foundAssistantId) = await userRepository.FindUserId(updateGame.AssistantLogin);
                if (assistantExists)
                {
                    changes.Field(g => g.AssistantId, null);
                    await assignmentService.CreateAssignment(game.Id, foundAssistantId);
                }
            }

            var invokedEvents = new List<EventType> {EventType.ChangedGame};
            if (updateGame.Status.HasValue && updateGame.Status != game.Status)
            {
                var (intention, eventType) = intentionConverter.Convert(updateGame.Status.Value);
                if (await intentionManager.IsAllowed(intention, game))
                {
                    changes.Field(g => g.Status, updateGame.Status.Value);
                    invokedEvents.Add(eventType);
                    
                    if (updateGame.Status == GameStatus.Moderation) // when we go to moderation the actor becomes nanny
                    {
                        changes = changes.Field(g => g.NannyId, identity.User.UserId);
                    }
                    else if (game.Status == GameStatus.Moderation) // when we go from moderation the nanny is no more
                    {
                        changes = changes.Field(g => g.NannyId, null);
                    }

                    if (!game.ReleaseDate.HasValue && updateGame.Status == GameStatus.Requirement)
                    {
                        changes = changes.Field(g => g.ReleaseDate, dateTimeProvider.Now);
                    }
                }
            }

            var result = await updatingRepository.Update(changes);
            await publisher.Publish(invokedEvents, game.Id);
            return result;
        }
    }
}
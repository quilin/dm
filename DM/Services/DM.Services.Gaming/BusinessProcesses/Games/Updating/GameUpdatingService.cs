using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Games.AssistantAssignment;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using FluentValidation;
using Game = DM.Services.DataAccess.BusinessObjects.Games.Game;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating
{
    /// <inheritdoc />
    public class GameUpdatingService : IGameUpdatingService
    {
        private readonly IValidator<UpdateGame> validator;
        private readonly IIntentionManager intentionManager;
        private readonly IIdentityProvider identityProvider;
        private readonly IGameReadingService gameReadingService;
        private readonly IAssignmentService assignmentService;
        private readonly IGameStateTransition gameStateTransition;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IUserRepository userRepository;
        private readonly IGameUpdatingRepository updatingRepository;

        /// <inheritdoc />
        public GameUpdatingService(
            IValidator<UpdateGame> validator,
            IIntentionManager intentionManager,
            IIdentityProvider identityProvider,
            IGameReadingService gameReadingService,
            IAssignmentService assignmentService,
            IGameStateTransition gameStateTransition,
            IUpdateBuilderFactory updateBuilderFactory,
            IUserRepository userRepository,
            IGameUpdatingRepository updatingRepository)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.identityProvider = identityProvider;
            this.gameReadingService = gameReadingService;
            this.assignmentService = assignmentService;
            this.gameStateTransition = gameStateTransition;
            this.updateBuilderFactory = updateBuilderFactory;
            this.userRepository = userRepository;
            this.updatingRepository = updatingRepository;
        }

        /// <inheritdoc />
        public async Task<GameExtended> Update(UpdateGame updateGame)
        {
            await validator.ValidateAndThrowAsync(updateGame);
            var game = await gameReadingService.GetGame(updateGame.GameId);
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

            if (updateGame.Status.HasValue && updateGame.Status.Value != game.Status)
            {
                var (success, assignNanny) = gameStateTransition.TryChange(game, updateGame.Status.Value);
                if (success)
                {
                    changes = changes.Field(c => c.Status, updateGame.Status.Value);
                    if (assignNanny.HasValue)
                    {
                        var nannyId = assignNanny.Value ? identityProvider.Current.User.UserId : (Guid?) null;
                        changes = changes.Field(c => c.NannyId, nannyId);
                    }
                }
            }

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

            return await updatingRepository.Update(changes);
        }
    }
}
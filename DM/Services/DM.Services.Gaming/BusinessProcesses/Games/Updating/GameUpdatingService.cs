using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.DataAccess.BusinessObjects.Users;
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
        private readonly IGameReadingService gameReadingService;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IAssistantAssignmentTokenFactory assignmentTokenFactory;
        private readonly IUserRepository userRepository;
        private readonly IGameUpdatingRepository updatingRepository;

        /// <inheritdoc />
        public GameUpdatingService(
            IValidator<UpdateGame> validator,
            IIntentionManager intentionManager,
            IGameReadingService gameReadingService,
            IUpdateBuilderFactory updateBuilderFactory,
            IAssistantAssignmentTokenFactory assignmentTokenFactory,
            IUserRepository userRepository,
            IGameUpdatingRepository updatingRepository)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.gameReadingService = gameReadingService;
            this.updateBuilderFactory = updateBuilderFactory;
            this.assignmentTokenFactory = assignmentTokenFactory;
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
                .MaybeField(c => c.DisableAlignment, updateGame.DisableAlignment)
                .MaybeField(c => c.Status, updateGame.Status); // TODO: status validation

            Token assistantAssignmentToken = null;
            if (updateGame.AssistantLogin != default)
            {
                var (assistantExists, foundAssistantId) = await userRepository.FindUserId(updateGame.AssistantLogin);
                if (assistantExists)
                {
                    assistantAssignmentToken = assignmentTokenFactory.Create(foundAssistantId, game.Id);
                }
            }

            return await updatingRepository.Update(changes, assistantAssignmentToken);
        }
    }
}
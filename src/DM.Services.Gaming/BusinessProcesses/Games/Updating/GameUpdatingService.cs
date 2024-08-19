using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;
using Game = DM.Services.DataAccess.BusinessObjects.Games.Game;

namespace DM.Services.Gaming.BusinessProcesses.Games.Updating;

/// <inheritdoc />
internal class GameUpdatingService(
    IValidator<UpdateGame> validator,
    IIntentionManager intentionManager,
    IGameReadingService gameReadingService,
    IAssignmentService assignmentService,
    IUpdateBuilderFactory updateBuilderFactory,
    IUserRepository userRepository,
    IDateTimeProvider dateTimeProvider,
    IGameUpdatingRepository updatingRepository,
    IGameIntentionConverter intentionConverter,
    IInvokedEventProducer producer,
    IIdentityProvider identityProvider) : IGameUpdatingService
{
    /// <inheritdoc />
    public async Task<GameExtended> Update(UpdateGame updateGame, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(updateGame, cancellationToken);
        var game = await gameReadingService.GetGameDetails(updateGame.GameId, cancellationToken);
        intentionManager.ThrowIfForbidden(GameIntention.Edit, game);

        var changes = updateBuilderFactory.Create<Game>(game.Id)
            .MaybeField(c => c.Title, updateGame.Title?.Trim())
            .MaybeField(c => c.SettingName, updateGame.SettingName?.Trim())
            .MaybeField(c => c.SystemName, updateGame.SystemName?.Trim())
            .MaybeField(c => c.Info, updateGame.Info)
            .MaybeField(c => c.HideTemper, updateGame.HideTemper)
            .MaybeField(c => c.HideStory, updateGame.HideStory)
            .MaybeField(c => c.HideSkills, updateGame.HideSkills)
            .MaybeField(c => c.HideInventory, updateGame.HideInventory)
            .MaybeField(c => c.HideDiceResult, updateGame.HideDiceResult)
            .MaybeField(c => c.ShowPrivateMessages, updateGame.ShowPrivateMessages)
            .MaybeField(c => c.CommentariesAccessMode, updateGame.CommentariesAccessMode)
            .MaybeField(c => c.DisableAlignment, updateGame.DisableAlignment)
            .MaybeField(c => c.Notepad, updateGame.Notepad);

        var oldAssistant = game.Assistant ?? game.PendingAssistant;
        if (updateGame.AssistantLogin != default &&
            !updateGame.AssistantLogin.Equals(oldAssistant?.Login, StringComparison.InvariantCultureIgnoreCase))
        {
            var (assistantExists, foundAssistantId) = await userRepository.FindUserId(
                updateGame.AssistantLogin, cancellationToken);
            if (assistantExists)
            {
                changes.Field(g => g.AssistantId, null);
                await assignmentService.CreateAssignment(game.Id, foundAssistantId, cancellationToken);
            }
        }

        var invokedEvents = new List<EventType> {EventType.ChangedGame};
        if (updateGame.Status.HasValue && updateGame.Status != game.Status)
        {
            var (intention, eventType) = intentionConverter.Convert(updateGame.Status.Value);
            if (intentionManager.IsAllowed(intention, game))
            {
                changes.Field(g => g.Status, updateGame.Status.Value);
                invokedEvents.Add(eventType);
                    
                if (updateGame.Status == GameStatus.Moderation) // when we go to moderation the actor becomes nanny
                {
                    changes = changes.Field(g => g.NannyId, identityProvider.Current.User.UserId);
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

        var result = await updatingRepository.Update(changes, cancellationToken);
        await producer.Send(invokedEvents, game.Id);
        return result;
    }
}
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Chat.Reading;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Chat.Creating;

/// <inheritdoc />
internal class ChatCreatingService(
    IValidator<CreateChatMessage> validator,
    IIntentionManager intentionManager,
    IChatMessageFactory factory,
    IChatCreatingRepository repository,
    IInvokedEventProducer producer,
    IIdentityProvider identityProvider) : IChatCreatingService
{
    /// <inheritdoc />
    public async Task<ChatMessage> Create(CreateChatMessage createChatMessage, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createChatMessage, cancellationToken);
        intentionManager.ThrowIfForbidden(ChatIntention.CreateMessage);

        var chatMessage = factory.Create(createChatMessage, identityProvider.Current.User.UserId);
        var result = await repository.Create(chatMessage, cancellationToken);
        await producer.Send(EventType.NewChatMessage, chatMessage.ChatMessageId);

        return result;
    }
}
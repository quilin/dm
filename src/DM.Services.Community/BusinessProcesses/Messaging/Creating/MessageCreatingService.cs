using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;
using DbConversation = DM.Services.DataAccess.BusinessObjects.Messaging.Conversation;

namespace DM.Services.Community.BusinessProcesses.Messaging.Creating;

/// <inheritdoc />
internal class MessageCreatingService(
    IConversationReadingService conversationReadingService,
    IValidator<CreateMessage> validator,
    IIntentionManager intentionManager,
    IMessageFactory factory,
    IUpdateBuilderFactory updateBuilderFactory,
    IMessageCreatingRepository repository,
    IUnreadCountersRepository unreadCountersRepository,
    IInvokedEventProducer producer,
    IIdentityProvider identityProvider) : IMessageCreatingService
{
    /// <inheritdoc />
    public async Task<Message> Create(CreateMessage createMessage, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createMessage, cancellationToken);
        var conversation = await conversationReadingService.Get(createMessage.ConversationId, cancellationToken);
        intentionManager.ThrowIfForbidden(ConversationIntention.CreateMessage, conversation);

        var message = factory.Create(createMessage, identityProvider.Current.User.UserId);
        var updateConversation = updateBuilderFactory.Create<DbConversation>(conversation.Id)
            .Field(c => c.LastMessageId, message.MessageId);

        var result = await repository.Create(message, updateConversation, cancellationToken);
        await unreadCountersRepository.Increment(conversation.Id, UnreadEntryType.Message, cancellationToken);
        await producer.Send(EventType.NewMessage, message.MessageId);

        return result;
    }
}
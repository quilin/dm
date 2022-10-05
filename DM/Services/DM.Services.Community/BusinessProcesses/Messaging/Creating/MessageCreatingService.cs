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
internal class MessageCreatingService : IMessageCreatingService
{
    private readonly IConversationReadingService conversationReadingService;
    private readonly IValidator<CreateMessage> validator;
    private readonly IIntentionManager intentionManager;
    private readonly IMessageFactory factory;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IMessageCreatingRepository repository;
    private readonly IUnreadCountersRepository unreadCountersRepository;
    private readonly IInvokedEventProducer producer;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public MessageCreatingService(
        IConversationReadingService conversationReadingService,
        IValidator<CreateMessage> validator,
        IIntentionManager intentionManager,
        IMessageFactory factory,
        IUpdateBuilderFactory updateBuilderFactory,
        IMessageCreatingRepository repository,
        IUnreadCountersRepository unreadCountersRepository,
        IInvokedEventProducer producer,
        IIdentityProvider identityProvider)
    {
        this.conversationReadingService = conversationReadingService;
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.factory = factory;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
        this.unreadCountersRepository = unreadCountersRepository;
        this.producer = producer;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<Message> Create(CreateMessage createMessage)
    {
        await validator.ValidateAndThrowAsync(createMessage);
        var conversation = await conversationReadingService.Get(createMessage.ConversationId);
        intentionManager.ThrowIfForbidden(ConversationIntention.CreateMessage, conversation);

        var message = factory.Create(createMessage, identityProvider.Current.User.UserId);
        var updateConversation = updateBuilderFactory.Create<DbConversation>(conversation.Id)
            .Field(c => c.LastMessageId, message.MessageId);

        var result = await repository.Create(message, updateConversation);
        await unreadCountersRepository.Increment(conversation.Id, UnreadEntryType.Message);
        await producer.Send(EventType.NewMessage, message.MessageId);

        return result;
    }
}
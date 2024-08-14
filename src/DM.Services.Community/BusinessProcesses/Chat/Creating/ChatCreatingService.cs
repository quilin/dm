using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Chat.Reading;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Chat.Creating;

/// <inheritdoc />
internal class ChatCreatingService : IChatCreatingService
{
    private readonly IValidator<CreateChatMessage> validator;
    private readonly IIntentionManager intentionManager;
    private readonly IChatMessageFactory factory;
    private readonly IChatCreatingRepository repository;
    private readonly IInvokedEventProducer producer;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public ChatCreatingService(
        IValidator<CreateChatMessage> validator,
        IIntentionManager intentionManager,
        IChatMessageFactory factory,
        IChatCreatingRepository repository,
        IInvokedEventProducer producer,
        IIdentityProvider identityProvider)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.factory = factory;
        this.repository = repository;
        this.producer = producer;
        this.identityProvider = identityProvider;
    }
        
    /// <inheritdoc />
    public async Task<ChatMessage> Create(CreateChatMessage createChatMessage)
    {
        await validator.ValidateAndThrowAsync(createChatMessage);
        intentionManager.ThrowIfForbidden(ChatIntention.CreateMessage);

        var chatMessage = factory.Create(createChatMessage, identityProvider.Current.User.UserId);
        var result = await repository.Create(chatMessage);
        await producer.Send(EventType.NewChatMessage, chatMessage.ChatMessageId);

        return result;
    }
}
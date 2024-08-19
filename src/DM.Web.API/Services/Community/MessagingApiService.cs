using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Messaging.Creating;
using DM.Services.Community.BusinessProcesses.Messaging.Deleting;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using Conversation = DM.Web.API.Dto.Messaging.Conversation;
using Message = DM.Web.API.Dto.Messaging.Message;

namespace DM.Web.API.Services.Community;

/// <inheritdoc />
internal class MessagingApiService(
    IConversationReadingService conversationReadingService,
    IMessageReadingService messageReadingService,
    IMessageCreatingService messageCreatingService,
    IMessageDeletingService messageDeletingService,
    IMapper mapper) : IMessagingApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<Conversation>> GetConversations(
        PagingQuery query, CancellationToken cancellationToken)
    {
        var (conversations, paging) = await conversationReadingService.Get(query, cancellationToken);
        return new ListEnvelope<Conversation>(conversations.Select(mapper.Map<Conversation>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<Conversation>> GetConversation(
        Guid id, CancellationToken cancellationToken)
    {
        var conversation = await conversationReadingService.Get(id, cancellationToken);
        return new Envelope<Conversation>(mapper.Map<Conversation>(conversation));
    }

    /// <inheritdoc />
    public async Task<Envelope<Conversation>> GetConversation(
        string login, CancellationToken cancellationToken)
    {
        var conversation = await conversationReadingService.GetOrCreate(login, cancellationToken);
        return new Envelope<Conversation>(mapper.Map<Conversation>(conversation));
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<Message>> GetMessages(
        Guid conversationId, PagingQuery query, CancellationToken cancellationToken)
    {
        var (messages, paging) = await messageReadingService.Get(conversationId, query, cancellationToken);
        return new ListEnvelope<Message>(messages.Select(mapper.Map<Message>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<Message>> CreateMessage(
        Guid conversationId, Message message, CancellationToken cancellationToken)
    {
        var createMessage = mapper.Map<CreateMessage>(message);
        createMessage.ConversationId = conversationId;
        var createdMessage = await messageCreatingService.Create(createMessage, cancellationToken);
        return new Envelope<Message>(mapper.Map<Message>(createdMessage));
    }

    /// <inheritdoc />
    public async Task<Envelope<Message>> GetMessage(Guid messageId, CancellationToken cancellationToken)
    {
        var message = await messageReadingService.Get(messageId, cancellationToken);
        return new Envelope<Message>(mapper.Map<Message>(message));
    }

    /// <inheritdoc />
    public Task DeleteMessage(Guid messageId, CancellationToken cancellationToken) =>
        messageDeletingService.Delete(messageId, cancellationToken);

    /// <inheritdoc />
    public Task MarkAsRead(Guid conversationId, CancellationToken cancellationToken) =>
        conversationReadingService.MarkAsRead(conversationId, cancellationToken);
}
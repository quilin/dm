using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Chat.Creating;
using DM.Services.Community.BusinessProcesses.Chat.Reading;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using ChatMessage = DM.Web.API.Dto.Messaging.ChatMessage;

namespace DM.Web.API.Services.Community;

/// <inheritdoc />
internal class ChatApiService(
    IChatReadingService readingService,
    IChatCreatingService creatingService,
    IMapper mapper) : IChatApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<ChatMessage>> GetMessages(
        PagingQuery query, CancellationToken cancellationToken)
    {
        var (messages, paging) = await readingService.GetMessages(query, cancellationToken);
        return new ListEnvelope<ChatMessage>(messages.Select(mapper.Map<ChatMessage>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<ChatMessage>> CreateMessage(
        ChatMessage message, CancellationToken cancellationToken)
    {
        var createMessage = mapper.Map<CreateChatMessage>(message);
        var createdMessage = await creatingService.Create(createMessage, cancellationToken);
        return new Envelope<ChatMessage>(mapper.Map<ChatMessage>(createdMessage));
    }

    /// <inheritdoc />
    public async Task<Envelope<ChatMessage>> GetMessage(
        Guid id, CancellationToken cancellationToken)
    {
        var message = await readingService.GetMessage(id, cancellationToken);
        return new Envelope<ChatMessage>(mapper.Map<ChatMessage>(message));
    }
}
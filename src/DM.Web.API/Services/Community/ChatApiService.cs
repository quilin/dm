using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Chat.Creating;
using DM.Services.Community.BusinessProcesses.Chat.Reading;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using ChatMessage = DM.Web.API.Dto.Messaging.ChatMessage;

namespace DM.Web.API.Services.Community;

/// <inheritdoc />
internal class ChatApiService : IChatApiService
{
    private readonly IChatReadingService readingService;
    private readonly IChatCreatingService creatingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public ChatApiService(
        IChatReadingService readingService,
        IChatCreatingService creatingService,
        IMapper mapper)
    {
        this.readingService = readingService;
        this.creatingService = creatingService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<ChatMessage>> GetMessages(PagingQuery query)
    {
        var (messages, paging) = await readingService.GetMessages(query);
        return new ListEnvelope<ChatMessage>(messages.Select(mapper.Map<ChatMessage>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<ChatMessage>> CreateMessage(ChatMessage message)
    {
        var createMessage = mapper.Map<CreateChatMessage>(message);
        var createdMessage = await creatingService.Create(createMessage);
        return new Envelope<ChatMessage>(mapper.Map<ChatMessage>(createdMessage));
    }

    /// <inheritdoc />
    public async Task<Envelope<ChatMessage>> GetMessage(Guid id)
    {
        var message = await readingService.GetMessage(id);
        return new Envelope<ChatMessage>(mapper.Map<ChatMessage>(message));
    }
}
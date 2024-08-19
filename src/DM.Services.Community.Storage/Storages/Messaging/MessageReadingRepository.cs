using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.Storage.Storages.Messaging;

/// <inheritdoc />
internal class MessageReadingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IMessageReadingRepository
{
    /// <inheritdoc />
    public Task<int> Count(Guid conversationId, CancellationToken cancellationToken) =>
        dbContext.Messages
            .Where(m => !m.IsRemoved && m.ConversationId == conversationId)
            .CountAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Message>> Get(Guid conversationId, PagingData paging,
        CancellationToken cancellationToken) =>
        await dbContext.Messages
            .Where(m => !m.IsRemoved && m.ConversationId == conversationId)
            .OrderBy(m => m.CreateDate)
            .Page(paging)
            .ProjectTo<Message>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

    /// <inheritdoc />
    public Task<Message?> Get(Guid messageId, Guid userId, CancellationToken cancellationToken) =>
        dbContext.Conversations
            .Where(ConversationReadingRepository.UserParticipates(userId))
            .SelectMany(c => c.Messages)
            .Where(m => !m.IsRemoved && m.MessageId == messageId)
            .ProjectTo<Message>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}
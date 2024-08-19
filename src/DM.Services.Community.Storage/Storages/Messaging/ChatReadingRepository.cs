using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Chat.Reading;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.Storage.Storages.Messaging;

/// <inheritdoc />
internal class ChatReadingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IChatReadingRepository
{
    /// <param name="cancellationToken"></param>
    /// <inheritdoc />
    public Task<int> Count(CancellationToken cancellationToken) => dbContext.ChatMessages.CountAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<ChatMessage>> Get(PagingData pagingData, CancellationToken cancellationToken) =>
        await dbContext.ChatMessages
            .OrderByDescending(m => m.CreateDate)
            .Page(pagingData)
            .ProjectTo<ChatMessage>(mapper.ConfigurationProvider)
            .OrderBy(m => m.CreateDate)
            .ToArrayAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<ChatMessage>> Get(DateTimeOffset since, CancellationToken cancellationToken) =>
        await dbContext.ChatMessages
            .OrderByDescending(m => m.CreateDate)
            .Where(m => m.CreateDate >= since)
            .ProjectTo<ChatMessage>(mapper.ConfigurationProvider)
            .OrderBy(m => m.CreateDate)
            .ToArrayAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<ChatMessage?> Get(Guid id, CancellationToken cancellationToken) =>
        await dbContext.ChatMessages
            .Where(m => m.ChatMessageId == id)
            .ProjectTo<ChatMessage>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Chat.Reading;

/// <inheritdoc />
internal class ChatReadingRepository : IChatReadingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public ChatReadingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public Task<int> Count() => dbContext.ChatMessages.CountAsync();

    /// <inheritdoc />
    public async Task<IEnumerable<ChatMessage>> Get(PagingData pagingData) => await dbContext.ChatMessages
        .OrderByDescending(m => m.CreateDate)
        .Page(pagingData)
        .ProjectTo<ChatMessage>(mapper.ConfigurationProvider)
        .OrderBy(m => m.CreateDate)
        .ToArrayAsync();

    /// <inheritdoc />
    public async Task<IEnumerable<ChatMessage>> Get(DateTimeOffset since) =>
        await dbContext.ChatMessages
            .OrderByDescending(m => m.CreateDate)
            .Where(m => m.CreateDate >= since)
            .ProjectTo<ChatMessage>(mapper.ConfigurationProvider)
            .OrderBy(m => m.CreateDate)
            .ToArrayAsync();

    /// <inheritdoc />
    public async Task<ChatMessage> Get(Guid id) => await dbContext.ChatMessages
        .Where(m => m.ChatMessageId == id)
        .ProjectTo<ChatMessage>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();
}
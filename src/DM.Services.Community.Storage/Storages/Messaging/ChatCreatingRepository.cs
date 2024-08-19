using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Chat.Creating;
using DM.Services.Community.BusinessProcesses.Chat.Reading;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using DbMessage = DM.Services.DataAccess.BusinessObjects.Common.ChatMessage;

namespace DM.Services.Community.Storage.Storages.Messaging;

/// <inheritdoc />
internal class ChatCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IChatCreatingRepository
{
    /// <inheritdoc />
    public async Task<ChatMessage> Create(DbMessage chatMessage, CancellationToken cancellationToken)
    {
        dbContext.ChatMessages.Add(chatMessage);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.ChatMessages
            .Where(m => m.ChatMessageId == chatMessage.ChatMessageId)
            .ProjectTo<ChatMessage>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}
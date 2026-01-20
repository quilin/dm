using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Messaging.Creating;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;
using DbConversation = DM.Services.DataAccess.BusinessObjects.Messaging.Conversation;
using DbMessage = DM.Services.DataAccess.BusinessObjects.Messaging.Message;

namespace DM.Services.Community.Storage.Storages.Messaging;

/// <inheritdoc />
internal class MessageCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IMessageCreatingRepository
{
    /// <inheritdoc />
    public async Task<Message> Create(
        DbMessage message, IUpdateBuilder<DbConversation> updateConversation, CancellationToken cancellationToken)
    {
        dbContext.Messages.Add(message);
        updateConversation.AttachTo(dbContext);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Messages
            .Where(m => m.MessageId == message.MessageId)
            .ProjectTo<Message>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}
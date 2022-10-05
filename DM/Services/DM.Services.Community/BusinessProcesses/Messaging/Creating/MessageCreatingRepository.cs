using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;
using DbConversation = DM.Services.DataAccess.BusinessObjects.Messaging.Conversation;
using DbMessage = DM.Services.DataAccess.BusinessObjects.Messaging.Message;

namespace DM.Services.Community.BusinessProcesses.Messaging.Creating;

/// <inheritdoc />
internal class MessageCreatingRepository : IMessageCreatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public MessageCreatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public async Task<Message> Create(DbMessage message, IUpdateBuilder<DbConversation> updateConversation)
    {
        dbContext.Messages.Add(message);
        updateConversation.AttachTo(dbContext);
        await dbContext.SaveChangesAsync();

        return await dbContext.Messages
            .Where(m => m.MessageId == message.MessageId)
            .ProjectTo<Message>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}
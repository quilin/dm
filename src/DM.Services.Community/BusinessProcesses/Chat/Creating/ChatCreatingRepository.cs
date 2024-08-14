using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Chat.Reading;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Chat.Creating;

/// <inheritdoc />
internal class ChatCreatingRepository : IChatCreatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public ChatCreatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
        
    /// <inheritdoc />
    public async Task<ChatMessage> Create(DataAccess.BusinessObjects.Common.ChatMessage chatMessage)
    {
        dbContext.ChatMessages.Add(chatMessage);
        await dbContext.SaveChangesAsync();

        return await dbContext.ChatMessages
            .Where(m => m.ChatMessageId == chatMessage.ChatMessageId)
            .ProjectTo<ChatMessage>(mapper.ConfigurationProvider)
            .FirstAsync();
    }
}
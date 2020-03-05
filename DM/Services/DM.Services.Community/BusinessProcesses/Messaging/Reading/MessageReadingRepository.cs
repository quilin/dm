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

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading
{
    /// <inheritdoc />
    public class MessageReadingRepository : IMessageReadingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public MessageReadingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public Task<int> Count(Guid conversationId) => dbContext.Messages
            .Where(m => !m.IsRemoved && m.ConversationId == conversationId)
            .CountAsync();

        /// <inheritdoc />
        public async Task<IEnumerable<Message>> Get(Guid conversationId, PagingData paging) => await dbContext.Messages
            .Where(m => !m.IsRemoved && m.ConversationId == conversationId)
            .OrderBy(m => m.CreateDate)
            .Page(paging)
            .ProjectTo<Message>(mapper.ConfigurationProvider)
            .ToArrayAsync();

        /// <inheritdoc />
        public Task<Message> Get(Guid messageId, Guid userId) => dbContext.Messages
            .Where(m => !m.IsRemoved && m.MessageId == messageId)
            .Where(m => ConversationReadingRepository.UserParticipates(userId).Compile().Invoke(m.Conversation))
            .ProjectTo<Message>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }
}
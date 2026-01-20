using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Messaging;
using Microsoft.EntityFrameworkCore;
using Conversation = DM.Services.Community.BusinessProcesses.Messaging.Reading.Conversation;
using DbConversation = DM.Services.DataAccess.BusinessObjects.Messaging.Conversation;

namespace DM.Services.Community.Storage.Storages.Messaging;

/// <inheritdoc />
internal class ConversationReadingRepository(
    DmDbContext dbContext,
    IMapper mapper) : IConversationReadingRepository
{
    /// <summary>
    /// Participation predicate
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns></returns>
    public static Expression<Func<DbConversation, bool>> UserParticipates(Guid userId) =>
        c => c.UserLinks.Any(l => !l.IsRemoved && l.UserId == userId);

    /// <inheritdoc />
    public Task<int> Count(Guid userId, CancellationToken cancellationToken) => dbContext.Conversations
        .Where(UserParticipates(userId))
        .CountAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Conversation>> Get(Guid userId, PagingData paging,
        CancellationToken cancellationToken) =>
        await dbContext.Conversations
            .Where(UserParticipates(userId))
            .Where(c => c.LastMessageId.HasValue)
            .OrderByDescending(c => c.LastMessage.CreateDate)
            .Page(paging)
            .ProjectTo<Conversation>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

    /// <inheritdoc />
    public Task<Conversation?> Get(Guid conversationId, Guid userId, CancellationToken cancellationToken) =>
        dbContext.Conversations
            .Where(UserParticipates(userId))
            .ProjectTo<Conversation>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<Guid?> FindUser(string login, CancellationToken cancellationToken) =>
        (await dbContext.Users
            .Where(u => u.Login.ToLower() == login.ToLower() && !u.IsRemoved && u.Activated)
            .Select(u => new { u.UserId })
            .FirstOrDefaultAsync(cancellationToken))?.UserId;

    /// <inheritdoc />
    public Task<Conversation?> FindVisaviConversation(
        Guid userId, Guid visaviId, CancellationToken cancellationToken) =>
        dbContext.Conversations
            .Where(c => c.Visavi)
            .Where(UserParticipates(userId))
            .Where(UserParticipates(visaviId))
            .ProjectTo<Conversation>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<Conversation> Create(DbConversation conversation,
        IEnumerable<UserConversationLink> conversationLinks, CancellationToken cancellationToken)
    {
        dbContext.Conversations.Add(conversation);
        dbContext.UserConversationLinks.AddRange(conversationLinks);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Conversations
            .Where(c => c.ConversationId == conversation.ConversationId)
            .ProjectTo<Conversation>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}
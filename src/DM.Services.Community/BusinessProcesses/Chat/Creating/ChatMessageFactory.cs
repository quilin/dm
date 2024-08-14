using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Community.BusinessProcesses.Chat.Creating;

/// <inheritdoc />
internal class ChatMessageFactory : IChatMessageFactory
{
    private readonly IGuidFactory guidFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    /// <inheritdoc />
    public ChatMessageFactory(
        IGuidFactory guidFactory,
        IDateTimeProvider dateTimeProvider)
    {
        this.guidFactory = guidFactory;
        this.dateTimeProvider = dateTimeProvider;
    }
        
    /// <inheritdoc />
    public ChatMessage Create(CreateChatMessage createChatMessage, Guid userId)
    {
        return new ChatMessage
        {
            ChatMessageId = guidFactory.Create(),
            CreateDate = dateTimeProvider.Now,
            UserId = userId,
            Text = createChatMessage.Text.Trim()
        };
    }
}
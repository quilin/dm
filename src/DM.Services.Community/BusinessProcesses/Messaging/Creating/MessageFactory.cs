using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Messaging;

namespace DM.Services.Community.BusinessProcesses.Messaging.Creating;

/// <inheritdoc />
internal class MessageFactory : IMessageFactory
{
    private readonly IGuidFactory guidFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    /// <inheritdoc />
    public MessageFactory(
        IGuidFactory guidFactory,
        IDateTimeProvider dateTimeProvider)
    {
        this.guidFactory = guidFactory;
        this.dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public Message Create(CreateMessage createMessage, Guid userId) => new()
    {
        MessageId = guidFactory.Create(),
        ConversationId = createMessage.ConversationId,
        CreateDate = dateTimeProvider.Now,
        UserId = userId,
        Text = createMessage.Text,
        IsRemoved = false
    };
}
using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.Consumer.Implementation.Notifiers;

/// <inheritdoc />
public abstract class BaseNotificationGenerator : INotificationGenerator
{
    /// <summary>
    /// Event type that generator can process
    /// </summary>
    protected abstract EventType EventType { get; }

    /// <inheritdoc />
    public bool CanResolve(EventType eventType) => eventType == EventType;

    /// <summary>
    /// Generate DAL models of notifications to be stored
    /// </summary>
    /// <param name="entityId">Entity identifier</param>
    /// <returns></returns>
    public abstract IAsyncEnumerable<CreateNotification> Generate(Guid entityId);
}
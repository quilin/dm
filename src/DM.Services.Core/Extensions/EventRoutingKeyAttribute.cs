using System;

namespace DM.Services.Core.Extensions;

/// <summary>
/// Attribute for the event type routing key
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class EventRoutingKeyAttribute : Attribute
{
    /// <summary>
    /// Message routing key
    /// </summary>
    public string RoutingKey { get; }

    /// <inheritdoc />
    public EventRoutingKeyAttribute(string routingKey)
    {
        RoutingKey = routingKey;
    }
}
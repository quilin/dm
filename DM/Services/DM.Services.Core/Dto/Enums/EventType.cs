using DM.Services.Core.Extensions;

namespace DM.Services.Core.Dto.Enums
{
    /// <summary>
    /// Message queue event type
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// Unknown event
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// New user has been created
        /// </summary>
        [EventRoutingKey("new.user")]
        NewUser = 1,

        /// <summary>
        /// New topic has been created
        /// </summary>
        [EventRoutingKey("new.topic")]
        NewTopic = 1
    }
}
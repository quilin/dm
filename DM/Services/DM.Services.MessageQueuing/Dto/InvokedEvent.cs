using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.MessageQueuing.Dto
{
    /// <summary>
    /// DTO model for general DM event
    /// </summary>
    public class InvokedEvent
    {
        /// <summary>
        /// Event type (e.g. "new topic" or "character accepted")
        /// </summary>
        public EventType Type { get; set; }

        /// <summary>
        /// Entity identifier (e.g. "new topic id" or "character accepted id")
        /// </summary>
        public Guid EntityId { get; set; }
    }
}
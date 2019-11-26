using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.MessageQueuing.Publish
{
    /// <summary>
    /// Publisher for the general invoked event
    /// </summary>
    public interface IInvokedEventPublisher
    {
        /// <summary>
        /// Publish invoked event
        /// </summary>
        /// <param name="type">Event type</param>
        /// <param name="entityId">Entity identifier</param>
        /// <returns></returns>
        Task Publish(EventType type, Guid entityId);

        /// <summary>
        /// Publish multiple invoked events for single entity
        /// </summary>
        /// <param name="types">Event types</param>
        /// <param name="entityId">Entity identifier</param>
        /// <returns></returns>
        Task Publish(IEnumerable<EventType> types, Guid entityId);
    }
}
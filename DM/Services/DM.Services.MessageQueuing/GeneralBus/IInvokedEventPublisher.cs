using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.MessageQueuing.GeneralBus
{
    /// <summary>
    /// Продюсер событий общей шины
    /// </summary>
    public interface IInvokedEventPublisher
    {
        /// <summary>
        /// Опубликовать событие с сущностью
        /// </summary>
        /// <param name="eventType">Тип события</param>
        /// <param name="entityId">Идентификатор сущности</param>
        /// <returns></returns>
        Task Publish(EventType eventType, Guid entityId);

        /// <summary>
        /// Опубликовать набор событий с сущностью
        /// </summary>
        /// <param name="eventTypes">Типы событий</param>
        /// <param name="entityId">Идентификатор сущности</param>
        /// <returns></returns>
        Task Publish(IEnumerable<EventType> eventTypes, Guid entityId);
    }
}
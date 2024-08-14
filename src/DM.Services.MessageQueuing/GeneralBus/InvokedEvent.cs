using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.MessageQueuing.GeneralBus;

/// <summary>
/// Модель общего события системы
/// </summary>
public class InvokedEvent
{
    /// <summary>
    /// Тип события (например, "новая тема форума" или "персонаж принят в игру")
    /// </summary>
    public EventType Type { get; set; }

    /// <summary>
    /// Идентификатор объекта (например новой темы форума или принятого персонажа)
    /// </summary>
    public Guid EntityId { get; set; }
}
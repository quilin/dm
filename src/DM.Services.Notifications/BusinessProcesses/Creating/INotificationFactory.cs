using System;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.BusinessProcesses.Creating;

/// <summary>
/// Factory for notifications DAL model
/// </summary>
internal interface INotificationFactory
{
    /// <summary>
    /// Create new notification DAL model
    /// </summary>
    /// <param name="createNotification"></param>
    /// <param name="createDate">Common create date</param>
    /// <returns></returns>
    Notification Create(CreateNotification createNotification, DateTimeOffset createDate);
}
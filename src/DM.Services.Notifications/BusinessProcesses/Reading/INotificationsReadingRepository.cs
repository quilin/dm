using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.BusinessProcesses.Reading;

/// <summary>
/// Notifications reading storage
/// </summary>
internal interface INotificationsReadingRepository
{
    /// <summary>
    /// Counts user notifications
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns>All notifications count</returns>
    Task<long> Count(Guid userId);

    /// <summary>
    /// Counts unread notifications for user
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns>Unread notifications count</returns>
    Task<long> CountUnread(Guid userId);

    /// <summary>
    /// Gets paged notifications for user
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="pagingData">Paging data</param>
    /// <returns>List of notifications ordered by invocation date</returns>
    Task<IEnumerable<UserNotification>> GetNotifications(Guid userId, PagingData pagingData);
}
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.BusinessProcesses.Reading;

/// <summary>
/// Service for reading 
/// </summary>
public interface INotificationsReadingService
{
    /// <summary>
    /// Count unread notifications
    /// </summary>
    /// <returns></returns>
    Task<long> CountUnread();

    /// <summary>
    /// Get list of notifications
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<UserNotification>> Get(PagingQuery query);
}
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.BusinessProcesses.Reading;

/// <inheritdoc />
internal class NotificationsReadingService : INotificationsReadingService
{
    private readonly IIdentityProvider identityProvider;
    private readonly INotificationsReadingRepository repository;

    /// <inheritdoc />
    public NotificationsReadingService(
        IIdentityProvider identityProvider,
        INotificationsReadingRepository repository)
    {
        this.identityProvider = identityProvider;
        this.repository = repository;
    }

    /// <inheritdoc />
    public Task<long> CountUnread() => repository.CountUnread(identityProvider.Current.User.UserId);

    /// <inheritdoc />
    public async Task<IEnumerable<UserNotification>> Get(PagingQuery query)
    {
        var userId = identityProvider.Current.User.UserId;
        var totalCount = await repository.Count(userId);
        var pagingData = new PagingData(query, 10, (int) totalCount);
        return await repository.GetNotifications(userId, pagingData);
    }
}
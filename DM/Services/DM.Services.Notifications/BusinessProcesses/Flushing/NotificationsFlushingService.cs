using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;

namespace DM.Services.Notifications.BusinessProcesses.Flushing;

/// <inheritdoc />
internal class NotificationsFlushingService : INotificationsFlushingService
{
    private readonly IIdentityProvider identityProvider;
    private readonly INotificationsFlushingRepository repository;

    /// <inheritdoc />
    public NotificationsFlushingService(
        IIdentityProvider identityProvider,
        INotificationsFlushingRepository repository)
    {
        this.identityProvider = identityProvider;
        this.repository = repository;
    }

    /// <inheritdoc />
    public Task MarkAsRead(Guid notificationId) =>
        repository.MarkAsRead(notificationId, identityProvider.Current.User.UserId);

    /// <inheritdoc />
    public Task MarkAllAsRead() =>
        repository.MarkAllAsRead(identityProvider.Current.User.UserId);
}
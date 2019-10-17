using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;

namespace DM.Services.Notifications.BusinessProcesses.Flushing
{
    /// <inheritdoc />
    public class NotificationsFlushingService : INotificationsFlushingService
    {
        private readonly IIdentity identity;
        private readonly INotificationsFlushingRepository repository;

        /// <inheritdoc />
        public NotificationsFlushingService(
            IIdentityProvider identityProvider,
            INotificationsFlushingRepository repository)
        {
            identity = identityProvider.Current;
            this.repository = repository;
        }

        /// <inheritdoc />
        public Task MarkAsRead(Guid notificationId) =>
            repository.MarkAsRead(notificationId, identity.User.UserId);

        /// <inheritdoc />
        public Task MarkAllAsRead() =>
            repository.MarkAllAsRead(identity.User.UserId);
    }
}
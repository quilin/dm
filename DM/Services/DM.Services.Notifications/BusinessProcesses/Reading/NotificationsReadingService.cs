using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.BusinessProcesses.Reading
{
    /// <inheritdoc />
    public class NotificationsReadingService : INotificationsReadingService
    {
        private readonly IIdentity identity;
        private readonly INotificationsReadingRepository repository;

        /// <inheritdoc />
        public NotificationsReadingService(
            IIdentityProvider identityProvider,
            INotificationsReadingRepository repository)
        {
            identity = identityProvider.Current;
            this.repository = repository;
        }

        /// <inheritdoc />
        public Task<long> CountUnread() => repository.CountUnread(identity.User.UserId);

        /// <inheritdoc />
        public async Task<IEnumerable<UserNotification>> Get(PagingQuery query)
        {
            var userId = identity.User.UserId;
            var totalCount = await repository.Count(userId);
            var pagingData = new PagingData(query, 10, (int) totalCount);
            return await repository.GetNotifications(userId, pagingData);
        }
    }
}
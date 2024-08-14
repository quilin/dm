using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.BusinessProcesses.Creating;

/// <inheritdoc />
internal class NotificationCreatingService : INotificationCreatingService
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly INotificationFactory factory;
    private readonly INotificationCreatingRepository repository;

    /// <inheritdoc />
    public NotificationCreatingService(
        IDateTimeProvider dateTimeProvider,
        INotificationFactory factory,
        INotificationCreatingRepository repository)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.factory = factory;
        this.repository = repository;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Notification>> Create(IEnumerable<CreateNotification> createNotifications)
    {
        var createDate = dateTimeProvider.Now;
        var notifications = createNotifications
            .Select(n => factory.Create(n, createDate))
            .ToArray();

        await repository.Create(notifications);

        return notifications;
    }
}
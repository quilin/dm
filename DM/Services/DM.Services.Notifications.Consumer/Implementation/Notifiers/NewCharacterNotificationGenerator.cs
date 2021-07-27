using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Notifications;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Notifications.Consumer.Implementation.Notifiers
{
    /// <inheritdoc />
    public class NewCharacterNotificationGenerator : BaseNotificationGenerator
    {
        private readonly DmDbContext dbContext;
        private readonly IGuidFactory guidFactory;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public NewCharacterNotificationGenerator(
            DmDbContext dbContext,
            IGuidFactory guidFactory,
            IDateTimeProvider dateTimeProvider)
        {
            this.dbContext = dbContext;
            this.guidFactory = guidFactory;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <inheritdoc />
        protected override EventType EventType => EventType.NewCharacter;

        /// <inheritdoc />
        protected override async Task<IEnumerable<Notification>> GenerateNotifications(Guid entityId)
        {
            var data = await dbContext.Characters
                .Where(c => c.CharacterId == entityId)
                .Select(c => new
                {
                    c.GameId,
                    c.Game.Title,
                    c.Author.Login,
                    c.Author.UserId,
                    c.Game.MasterId,
                    c.Game.AssistantId
                })
                .FirstAsync();
            var interestedUsers = new List<Guid> {data.MasterId};
            if (data.AssistantId.HasValue)
            {
                interestedUsers.Add(data.AssistantId.Value);
            }

            interestedUsers = interestedUsers.Where(u => u != data.UserId).ToList();
            if (interestedUsers.Count == 0)
            {
                return new Notification[0];
            }

            return new[]
            {
                new Notification
                {
                    NotificationId = guidFactory.Create(),
                    CreateDate = dateTimeProvider.Now.UtcDateTime,
                    UsersNotified = new List<Guid>(),
                    UsersInterested = interestedUsers,
                    Metadata = new
                    {
                        AuthorLogin = data.Login,
                        GameTitle = data.Title,
                        GameId = data.GameId.EncodeToReadable()
                    },
                    EventType = EventType
                }
            };
        }
    }
}
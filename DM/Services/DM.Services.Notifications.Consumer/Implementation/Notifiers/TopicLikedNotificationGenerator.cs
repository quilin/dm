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
    public class TopicLikedNotificationGenerator : BaseNotificationGenerator
    {
        private readonly DmDbContext dbContext;
        private readonly IGuidFactory guidFactory;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public TopicLikedNotificationGenerator(
            DmDbContext dbContext,
            IGuidFactory guidFactory,
            IDateTimeProvider dateTimeProvider)
        {
            this.dbContext = dbContext;
            this.guidFactory = guidFactory;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <inheritdoc />
        protected override EventType EventType => EventType.LikedTopic;

        /// <inheritdoc />
        protected override async Task<IEnumerable<Notification>> GenerateNotifications(Guid entityId)
        {
            var likedTopicData = await dbContext.Likes
                .Where(like => like.LikeId == entityId)
                .Select(like => new
                {
                    like.User.Login,
                    like.Topic.ForumTopicId,
                    like.Topic.UserId,
                    like.Topic.Title
                })
                .FirstAsync();

            return new[]
            {
                new Notification
                {
                    NotificationId = guidFactory.Create(),
                    CreateDate = dateTimeProvider.Now.UtcDateTime,
                    UsersNotified = new List<Guid>(),
                    UsersInterested = new[] {likedTopicData.UserId},
                    Metadata = new
                    {
                        AuthorLogin = likedTopicData.Login,
                        TopicTitle = likedTopicData.Title,
                        TopicId = likedTopicData.ForumTopicId.EncodeToReadable(likedTopicData.Title)
                    }
                }
            };
        }
    }
}
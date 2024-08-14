using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.Notifications.Dto;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Notifications.Consumer.Implementation.Notifiers.Forum;

/// <inheritdoc />
internal class TopicLikedNotificationGenerator : BaseNotificationGenerator
{
    private readonly DmDbContext dbContext;

    /// <inheritdoc />
    public TopicLikedNotificationGenerator(
        DmDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    protected override EventType EventType => EventType.LikedTopic;

    /// <inheritdoc />
    public override async IAsyncEnumerable<CreateNotification> Generate(Guid entityId)
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

        yield return new CreateNotification
        {
            UsersInterested = new[] {likedTopicData.UserId},
            Metadata = new
            {
                AuthorLogin = likedTopicData.Login,
                TopicTitle = likedTopicData.Title,
                TopicId = likedTopicData.ForumTopicId.EncodeToReadable(likedTopicData.Title)
            }
        };
    }
}
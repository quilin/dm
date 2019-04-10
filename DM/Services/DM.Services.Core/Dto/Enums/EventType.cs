using DM.Services.Core.Extensions;

namespace DM.Services.Core.Dto.Enums
{
    /// <summary>
    /// Message queue event type
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// Unknown event
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// New user has been created
        /// </summary>
        [EventRoutingKey("new.user")]
        NewUser = 1,

        /// <summary>
        /// New topic has been created
        /// </summary>
        [EventRoutingKey("new.topic")]
        NewTopic = 101,

        /// <summary>
        /// Topic text has been updated
        /// </summary>
        [EventRoutingKey("changed.topic")]
        ChangedTopic = 102,

        /// <summary>
        /// Topic has been deleted
        /// </summary>
        [EventRoutingKey("deleted.topic")]
        DeletedTopic = 104,

        /// <summary>
        /// New commentary has been created on forum
        /// </summary>
        [EventRoutingKey("new.comment.forum")]
        NewForumComment = 201,

        /// <summary>
        /// Forum commentary has been updated
        /// </summary>
        [EventRoutingKey("changed.comment.forum")]
        ChangedForumComment = 202,

        /// <summary>
        /// Forum commentary has been deleted
        /// </summary>
        [EventRoutingKey("deleted.comment.forum")]
        DeletedForumComment = 203,

        /// <summary>
        /// Topic has been liked
        /// </summary>
        [EventRoutingKey("new.like.topic")]
        LikedTopic = 601,

        /// <summary>
        /// Forum commentary has been liked
        /// </summary>
        [EventRoutingKey("new.like.comment.forum")]
        LikedForumComment = 602
    }
}
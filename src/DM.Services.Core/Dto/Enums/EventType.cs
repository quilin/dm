using DM.Services.Core.Extensions;

namespace DM.Services.Core.Dto.Enums;

/// <summary>
/// Message queue event type
///
/// 1-99    Community events
/// 100-199 Forum events
/// 200-299 TBD
/// 300-499 Game events
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
    [EventRoutingKey("community.user.registered")]
    NewUser = 1,

    /// <summary>
    /// User has been activated
    /// </summary>
    [EventRoutingKey("community.user.activated")]
    ActivatedUser = 2,

    /// <summary>
    /// New message has been sent
    /// </summary>
    [EventRoutingKey("messaging.message.created")]
    NewMessage = 11,

    /// <summary>
    /// New chat message has been sent
    /// </summary>
    [EventRoutingKey("chat.message.created")]
    NewChatMessage = 31,

    /// <summary>
    /// New poll has been published
    /// </summary>
    [EventRoutingKey("community.poll.created")]
    NewPoll = 51,

    /// <summary>
    /// New topic has been created
    /// </summary>
    [EventRoutingKey("forum.topic.created")]
    NewForumTopic = 101,

    /// <summary>
    /// Topic text has been updated
    /// </summary>
    [EventRoutingKey("forum.topic.changed")]
    ChangedForumTopic = 102,

    /// <summary>
    /// Topic has been deleted
    /// </summary>
    [EventRoutingKey("forum.topic.deleted")]
    DeletedForumTopic = 103,

    /// <summary>
    /// Topic has been liked
    /// </summary>
    [EventRoutingKey("forum.topic.liked")]
    LikedTopic = 104,

    /// <summary>
    /// New commentary has been created on forum
    /// </summary>
    [EventRoutingKey("forum.comment.created")]
    NewForumComment = 111,

    /// <summary>
    /// Forum commentary has been updated
    /// </summary>
    [EventRoutingKey("forum.comment.changed")]
    ChangedForumComment = 112,

    /// <summary>
    /// Forum commentary has been deleted
    /// </summary>
    [EventRoutingKey("forum.comment.deleted")]
    DeletedForumComment = 113,

    /// <summary>
    /// Forum commentary has been liked
    /// </summary>
    [EventRoutingKey("forum.comment.liked")]
    LikedForumComment = 114,

    /// <summary>
    /// New game has been created
    /// </summary>
    [EventRoutingKey("game.module.created")]
    NewGame = 301,

    /// <summary>
    /// Game has been updated
    /// </summary>
    [EventRoutingKey("game.module.changed")]
    ChangedGame = 302,

    /// <summary>
    /// Game has been deleted
    /// </summary>
    [EventRoutingKey("game.module.deleted")]
    DeletedGame = 303,

    /// <summary>
    /// Game is on moderation
    /// </summary>
    [EventRoutingKey("game.module.status.moderation")]
    StatusGameModeration = 321,

    /// <summary>
    /// Game is draft
    /// </summary>
    [EventRoutingKey("game.module.status.draft")]
    StatusGameDraft = 322,

    /// <summary>
    /// Game is released
    /// </summary>
    [EventRoutingKey("game.module.status.requirement")]
    StatusGameRequirement = 323,

    /// <summary>
    /// Game has started
    /// </summary>
    [EventRoutingKey("game.module.status.active")]
    StatusGameActive = 324,

    /// <summary>
    /// Game is frozen
    /// </summary>
    [EventRoutingKey("game.module.status.frozen")]
    StatusGameFrozen = 325,

    /// <summary>
    /// Game is finished
    /// </summary>
    [EventRoutingKey("game.module.status.finished")]
    StatusGameFinished = 326,

    /// <summary>
    /// Game is closed
    /// </summary>
    [EventRoutingKey("game.module.status.closed")]
    StatusGameClosed = 327,

    /// <summary>
    /// New commentary has been created on game
    /// </summary>
    [EventRoutingKey("game.comment.created")]
    NewGameComment = 331,

    /// <summary>
    /// Game commentary has been updated
    /// </summary>
    [EventRoutingKey("game.comment.changed")]
    ChangedGameComment = 332,

    /// <summary>
    /// Forum commentary has been deleted
    /// </summary>
    [EventRoutingKey("game.comment.deleted")]
    DeletedGameComment = 333,

    /// <summary>
    /// Game commentary has been liked
    /// </summary>
    [EventRoutingKey("game.comment.liked")]
    LikedGameComment = 334,

    /// <summary>
    /// Assistant assignment request created and pending
    /// </summary>
    [EventRoutingKey("game.module.assignment.created")]
    AssignmentRequestCreated = 351,

    /// <summary>
    /// Assistant assignment request has been accepted
    /// </summary>
    [EventRoutingKey("game.module.assignment.accepted")]
    AssignmentRequestAccepted = 352,

    /// <summary>
    /// Assistant assignment request has been rejected
    /// </summary>
    [EventRoutingKey("game.module.assignment.rejected")]
    AssignmentRequestRejected = 353,

    /// <summary>
    /// New character has been created
    /// </summary>
    [EventRoutingKey("game.character.created")]
    NewCharacter = 361,

    /// <summary>
    /// Character has been updated
    /// </summary>
    [EventRoutingKey("game.character.updated")]
    ChangedCharacter = 362,

    /// <summary>
    /// Character has been deleted
    /// </summary>
    [EventRoutingKey("game.character.deleted")]
    DeletedCharacter = 363,

    /// <summary>
    /// Character has been declined
    /// </summary>
    [EventRoutingKey("game.character.status.declined")]
    StatusCharacterDeclined = 371,

    /// <summary>
    /// Character has been accepted
    /// </summary>
    [EventRoutingKey("game.character.status.accepted")]
    StatusCharacterAccepted = 372,

    /// <summary>
    /// Character has been killed
    /// </summary>
    [EventRoutingKey("game.character.status.died")]
    StatusCharacterDied = 373,

    /// <summary>
    /// Character has been resurrected
    /// </summary>
    [EventRoutingKey("game.character.status.resurrected")]
    StatusCharacterResurrected = 374,

    /// <summary>
    /// Character has left the game
    /// </summary>
    [EventRoutingKey("game.character.status.left")]
    StatusCharacterLeft = 375,

    /// <summary>
    /// Character has returned to the game
    /// </summary>
    [EventRoutingKey("game.character.status.returned")]
    StatusCharacterReturned = 376,

    /// <summary>
    /// New room has been created
    /// </summary>
    [EventRoutingKey("game.room.created")]
    NewRoom = 381,

    /// <summary>
    /// Room has been updated
    /// </summary>
    [EventRoutingKey("game.room.updated")]
    ChangedRoom = 382,

    /// <summary>
    /// Room has been deleted
    /// </summary>
    [EventRoutingKey("game.room.deleted")]
    DeletedRoom = 383,

    /// <summary>
    /// New post pending has been created
    /// </summary>
    [EventRoutingKey("game.room.pending.created")]
    RoomPendingCreated = 384,

    /// <summary>
    /// Post pending has been responded
    /// </summary>
    [EventRoutingKey("game.room.pending.responded")]
    RoomPendingResponded = 385,

    /// <summary>
    /// New game post has been created
    /// </summary>
    [EventRoutingKey("game.post.created")]
    NewPost = 401,

    /// <summary>
    /// Post has been changed
    /// </summary>
    [EventRoutingKey("game.post.updated")]
    ChangedPost = 402,

    /// <summary>
    /// Post has been deleted
    /// </summary>
    [EventRoutingKey("game.post.deleted")]
    DeletedPost = 403,
}
using DM.Services.DataAccess.BusinessObjects.Administration;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Games.Rating;
using DM.Services.DataAccess.BusinessObjects.Messaging;
using DM.Services.DataAccess.BusinessObjects.Users;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.DataAccess;

/// <summary>
/// RDB storage context
/// </summary>
public class DmDbContext : DbContext
{
    /// <inheritdoc />
    public DmDbContext(DbContextOptions options) : base(options)
    {
    }

    #region Users

    /// <summary>
    /// Users
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// User authorization tokens
    /// </summary>
    public DbSet<Token> Tokens { get; set; }

    #endregion

    #region Common

    /// <summary>
    /// Commentaries
    /// </summary>
    public DbSet<Comment> Comments { get; set; }

    /// <summary>
    /// Likes
    /// </summary>
    public DbSet<Like> Likes { get; set; }

    /// <summary>
    /// Reviews
    /// </summary>
    public DbSet<Review> Reviews { get; set; }

    /// <summary>
    /// Tag groups
    /// </summary>
    public DbSet<TagGroup> TagGroups { get; set; }

    /// <summary>
    /// Tags
    /// </summary>
    public DbSet<Tag> Tags { get; set; }

    /// <summary>
    /// Uploads
    /// </summary>
    public DbSet<Upload> Uploads { get; set; }

    #endregion

    #region Forum

    /// <summary>
    /// Fora
    /// </summary>
    public DbSet<Forum> Fora { get; set; }

    /// <summary>
    /// Topics
    /// </summary>
    public DbSet<ForumTopic> ForumTopics { get; set; }

    /// <summary>
    /// Moderators
    /// </summary>
    public DbSet<ForumModerator> ForumModerators { get; set; }

    #endregion

    #region Games

    /// <summary>
    /// Games
    /// </summary>
    public DbSet<Game> Games { get; set; }

    /// <summary>
    /// Game tags
    /// </summary>
    public DbSet<GameTag> GameTags { get; set; }

    /// <summary>
    /// Game readers
    /// </summary>
    public DbSet<Reader> Readers { get; set; }

    /// <summary>
    /// Blacklists
    /// </summary>
    public DbSet<BlackListLink> BlackListLinks { get; set; }

    /// <summary>
    /// Characters in rooms
    /// </summary>
    public DbSet<RoomClaim> RoomClaims { get; set; }

    /// <summary>
    /// Characters
    /// </summary>
    public DbSet<Character> Characters { get; set; }

    /// <summary>
    /// Character attribute values
    /// </summary>
    public DbSet<CharacterAttribute> CharacterAttributes { get; set; }

    /// <summary>
    /// Game rooms
    /// </summary>
    public DbSet<Room> Rooms { get; set; }

    /// <summary>
    /// Game posts
    /// </summary>
    public DbSet<Post> Posts { get; set; }

    /// <summary>
    /// Game post anticipations
    /// </summary>
    public DbSet<PendingPost> PendingPosts { get; set; }

    /// <summary>
    /// Game post rating votes
    /// </summary>
    public DbSet<Vote> Votes { get; set; }

    #endregion

    #region Messaging

    /// <summary>
    /// Conversations
    /// </summary>
    public DbSet<Conversation> Conversations { get; set; }

    /// <summary>
    /// Conversations participants
    /// </summary>
    public DbSet<UserConversationLink> UserConversationLinks { get; set; }

    /// <summary>
    /// Conversation messages
    /// </summary>
    public DbSet<Message> Messages { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<ChatMessage> ChatMessages { get; set; }

    #endregion

    #region Administration

    /// <summary>
    /// Complaints
    /// </summary>
    public DbSet<Report> Reports { get; set; }

    /// <summary>
    /// Warnings
    /// </summary>
    public DbSet<Warning> Warnings { get; set; }

    /// <summary>
    /// Bans
    /// </summary>
    public DbSet<Ban> Bans { get; set; }

    #endregion
}
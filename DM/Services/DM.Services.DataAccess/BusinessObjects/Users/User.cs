using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Administration;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.DataAccess.BusinessObjects.Games;
using DM.Services.DataAccess.BusinessObjects.Games.Characters;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.BusinessObjects.Games.Posts;
using DM.Services.DataAccess.BusinessObjects.Games.Rating;
using DM.Services.DataAccess.BusinessObjects.Messaging;

namespace DM.Services.DataAccess.BusinessObjects.Users;

/// <summary>
/// DAL model for user
/// </summary>
[Table("Users")]
public class User : IUser, IRemovable
{
    /// <inheritdoc />
    [Key]
    public Guid UserId { get; set; }

    /// <inheritdoc />
    [MaxLength(100)]
    public string Login { get; set; }

    /// <summary>
    /// Registration email (unique)
    /// </summary>
    [MaxLength(100)]
    public string Email { get; set; }

    /// <summary>
    /// Registration moment
    /// </summary>
    public DateTimeOffset RegistrationDate { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? LastVisitDate { get; set; }

    /// <summary>
    /// Timezone identifier (OS-specific)
    /// </summary>
    [Obsolete("Need to remove it ASAP")]
    public string TimezoneId { get; set; }

    /// <inheritdoc />
    public UserRole Role { get; set; }

    /// <inheritdoc />
    public AccessPolicy AccessPolicy { get; set; }

    /// <summary>
    /// Password salt
    /// </summary>
    [MaxLength(120)]
    public string Salt { get; set; }

    /// <summary>
    /// Password hash
    /// </summary>
    [MaxLength(300)]
    public string PasswordHash { get; set; }

    /// <inheritdoc />
    public bool RatingDisabled { get; set; }

    /// <inheritdoc />
    public int QualityRating { get; set; }

    /// <inheritdoc />
    public int QuantityRating { get; set; }

    /// <summary>
    /// Activation flag
    /// </summary>
    public bool Activated { get; set; }

    /// <summary>
    /// DM2 account merge availability flag
    /// </summary>
    public bool CanMerge { get; set; }

    /// <summary>
    /// DM2 account identifier
    /// </summary>
    public Guid? MergeRequested { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <summary>
    /// Custom status
    /// </summary>
    [MaxLength(200)]
    public string Status { get; set; }

    /// <summary>
    /// Real name
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Real location
    /// </summary>
    [MaxLength(100)]
    public string Location { get; set; }

    /// <summary>
    /// ICQ number
    /// </summary>
    [MaxLength(20)]
    public string Icq { get; set; }

    /// <summary>
    /// Skype name
    /// </summary>
    [MaxLength(50)]
    public string Skype { get; set; }

    /// <summary>
    /// Full user information
    /// </summary>
    public string Info { get; set; }

    /// <summary>
    /// Profile picture original url
    /// </summary>
    [MaxLength(200)]
    public string ProfilePictureUrl { get; set; }

    /// <summary>
    /// Profile picture small cropped version url
    /// </summary>
    [MaxLength(200)]
    public string SmallProfilePictureUrl { get; set; }

    /// <summary>
    /// Profile picture medium cropped version url
    /// </summary>
    [MaxLength(200)]
    public string MediumProfilePictureUrl { get; set; }

    #region Profile navigations

    /// <summary>
    /// Profile picture (should only be one active)
    /// </summary>
    [InverseProperty(nameof(Upload.UserProfile))]
    public virtual ICollection<Upload> ProfilePictures { get; set; }

    /// <summary>
    /// Authorization tokens
    /// </summary>
    [InverseProperty(nameof(Token.User))]
    public virtual ICollection<Token> Tokens { get; set; }

    #endregion

    #region Common navigations

    /// <summary>
    /// User commentaries
    /// </summary>
    [InverseProperty(nameof(Comment.Author))]
    public virtual ICollection<Comment> Comments { get; set; }

    /// <summary>
    /// User likes
    /// </summary>
    [InverseProperty(nameof(Like.User))]
    public virtual ICollection<Like> Likes { get; set; }

    /// <summary>
    /// User reviews
    /// </summary>
    [InverseProperty(nameof(Review.Author))]
    public virtual ICollection<Review> Reviews { get; set; }

    /// <summary>
    /// User uploads
    /// </summary>
    [InverseProperty(nameof(Upload.Owner))]
    public virtual ICollection<Upload> Uploads { get; set; }

    #endregion

    #region Forum navigations

    /// <summary>
    /// User topics
    /// </summary>
    [InverseProperty(nameof(ForumTopic.Author))]
    public virtual ICollection<ForumTopic> Topics { get; set; }

    /// <summary>
    /// User moderation links
    /// </summary>
    [InverseProperty(nameof(ForumModerator.User))]
    public virtual ICollection<ForumModerator> ForumModerators { get; set; }

    #endregion

    #region Game navigations

    /// <summary>
    /// Games user is GM of
    /// </summary>
    [InverseProperty(nameof(Game.Master))]
    public virtual ICollection<Game> GamesAsMaster { get; set; }

    /// <summary>
    /// Games user is GM assistant of
    /// </summary>
    [InverseProperty(nameof(Game.Assistant))]
    public virtual ICollection<Game> GamesAsAssistant { get; set; }

    /// <summary>
    /// Games user moderates
    /// </summary>
    [InverseProperty(nameof(Game.Nanny))]
    public virtual ICollection<Game> GamesAsNanny { get; set; }

    /// <summary>
    /// Games user is blacklisted in
    /// </summary>
    [InverseProperty(nameof(BlackListLink.User))]
    public virtual ICollection<BlackListLink> GamesBlacklisted { get; set; }

    /// <summary>
    /// Games observed
    /// </summary>
    [InverseProperty(nameof(Reader.User))]
    public virtual ICollection<Reader> GamesObserved { get; set; }

    /// <summary>
    /// Characters
    /// </summary>
    [InverseProperty(nameof(Character.Author))]
    public virtual ICollection<Character> Characters { get; set; }

    /// <summary>
    /// Posts
    /// </summary>
    [InverseProperty(nameof(Post.Author))]
    public virtual ICollection<Post> Posts { get; set; }

    /// <summary>
    /// Post votes given
    /// </summary>
    [InverseProperty(nameof(Vote.VotedUser))]
    public virtual ICollection<Vote> VotesGiven { get; set; }

    /// <summary>
    /// Post votes received
    /// </summary>
    [InverseProperty(nameof(Vote.TargetUser))]
    public virtual ICollection<Vote> VotesReceived { get; set; }

    /// <summary>
    /// User waits for posts
    /// </summary>
    [InverseProperty(nameof(PendingPost.AwaitingUser))]
    public virtual ICollection<PendingPost> WaitsForPosts { get; set; }

    /// <summary>
    /// User posts are required
    /// </summary>
    [InverseProperty(nameof(PendingPost.PendingUser))]
    public virtual ICollection<PendingPost> PostsRequired { get; set; }

    #endregion

    #region Messaging navigations

    /// <summary>
    /// Conversation participations
    /// </summary>
    [InverseProperty(nameof(UserConversationLink.User))]
    public virtual ICollection<UserConversationLink> ConversationLinks { get; set; }

    /// <summary>
    /// Messages
    /// </summary>
    [InverseProperty(nameof(Message.Author))]
    public virtual ICollection<Message> Messages { get; set; }

    #endregion

    #region Administration navigations

    /// <summary>
    /// Reports given
    /// </summary>
    [InverseProperty(nameof(Report.Author))]
    public virtual ICollection<Report> ReportsGiven { get; set; }

    /// <summary>
    /// Reports taken
    /// </summary>
    [InverseProperty(nameof(Report.Target))]
    public virtual ICollection<Report> ReportsTaken { get; set; }

    /// <summary>
    /// Reports answered
    /// </summary>
    [InverseProperty(nameof(Report.AnswerAuthor))]
    public virtual ICollection<Report> ReportsAnswered { get; set; }

    /// <summary>
    /// Warnings received
    /// </summary>
    [InverseProperty(nameof(Warning.User))]
    public virtual ICollection<Warning> WarningsReceived { get; set; }

    /// <summary>
    /// Warnings given
    /// </summary>
    [InverseProperty(nameof(Warning.Moderator))]
    public virtual ICollection<Warning> WarningsGiven { get; set; }

    /// <summary>
    /// Bans received
    /// </summary>
    [InverseProperty(nameof(Ban.User))]
    public virtual ICollection<Ban> BansReceived { get; set; }

    /// <summary>
    /// Bans given
    /// </summary>
    [InverseProperty(nameof(Ban.Moderator))]
    public virtual ICollection<Ban> BansGiven { get; set; }

    #endregion
}
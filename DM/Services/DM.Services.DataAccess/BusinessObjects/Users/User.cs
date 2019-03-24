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

namespace DM.Services.DataAccess.BusinessObjects.Users
{
    [Table("Users")]
    public class User : IUser, IRemovable
    {
        [Key] public Guid UserId { get; set; }

        public string Login { get; set; }
        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public string TimezoneId { get; set; }

        public UserRole Role { get; set; }
        public AccessPolicy AccessPolicy { get; set; }

        public string Salt { get; set; }
        public string PasswordHash { get; set; }

        public bool RatingDisabled { get; set; }
        public int QualityRating { get; set; }
        public int QuantityRating { get; set; }

        public bool Activated { get; set; }
        public bool CanMerge { get; set; }
        public Guid? MergeRequested { get; set; }

        public bool IsRemoved { get; set; }

        #region Profile navigations

        [InverseProperty(nameof(UserProfile.User))]
        public virtual UserProfile Profile { get; set; }

        [InverseProperty(nameof(Upload.UserProfile))]
        public virtual ICollection<Upload> ProfilePictures { get; set; }

        [InverseProperty(nameof(Token.User))] public virtual ICollection<Token> Tokens { get; set; }

        #endregion

        #region Common navigations

        [InverseProperty(nameof(Comment.Author))]
        public virtual ICollection<Comment> Comments { get; set; }

        [InverseProperty(nameof(Like.User))] public virtual ICollection<Like> Likes { get; set; }

        [InverseProperty(nameof(Review.Author))]
        public virtual ICollection<Review> Reviews { get; set; }

        [InverseProperty(nameof(Upload.User))] public virtual ICollection<Upload> Uploads { get; set; }

        #endregion

        #region Forum navigations

        [InverseProperty(nameof(ForumTopic.Author))]
        public virtual ICollection<ForumTopic> Topics { get; set; }

        [InverseProperty(nameof(ForumModerator.User))]
        public virtual ICollection<ForumModerator> ForumModerators { get; set; }

        #endregion

        #region Game navigations

        [InverseProperty(nameof(Game.Master))] public virtual ICollection<Game> GamesAsMaster { get; set; }

        [InverseProperty(nameof(Game.Assistant))]
        public virtual ICollection<Game> GamesAsAssistant { get; set; }

        [InverseProperty(nameof(Game.Nanny))] public virtual ICollection<Game> GamesAsNanny { get; set; }

        [InverseProperty(nameof(BlackListLink.User))]
        public virtual ICollection<BlackListLink> GamesBlacklisted { get; set; }

        [InverseProperty(nameof(Reader.User))] public virtual ICollection<Reader> GamesObserved { get; set; }

        [InverseProperty(nameof(Character.User))]
        public virtual ICollection<Character> Characters { get; set; }

        [InverseProperty(nameof(Post.Author))] public virtual ICollection<Post> Posts { get; set; }

        [InverseProperty(nameof(Vote.VotedUser))]
        public virtual ICollection<Vote> VotesGiven { get; set; }

        [InverseProperty(nameof(Vote.TargetUser))]
        public virtual ICollection<Vote> VotesReceived { get; set; }

        [InverseProperty(nameof(PostAnticipation.User))]
        public virtual ICollection<PostAnticipation> WaitsForPosts { get; set; }

        [InverseProperty(nameof(PostAnticipation.Target))]
        public virtual ICollection<PostAnticipation> PostsAwaited { get; set; }

        #endregion

        #region Messaging navigations

        [InverseProperty(nameof(UserConversationLink.User))]
        public virtual ICollection<UserConversationLink> ConversationLinks { get; set; }

        [InverseProperty(nameof(Message.Author))]
        public virtual ICollection<Message> Messages { get; set; }

        #endregion

        #region Administration navigations

        [InverseProperty(nameof(Report.Author))]
        public virtual ICollection<Report> ReportsGiven { get; set; }

        [InverseProperty(nameof(Report.Target))]
        public virtual ICollection<Report> ReportsTaken { get; set; }

        [InverseProperty(nameof(Report.AnswerAuthor))]
        public virtual ICollection<Report> ReportsAnswered { get; set; }

        [InverseProperty(nameof(Warning.User))]
        public virtual ICollection<Warning> WarningsReceived { get; set; }

        [InverseProperty(nameof(Warning.Moderator))]
        public virtual ICollection<Warning> WarningsGiven { get; set; }

        [InverseProperty(nameof(Ban.User))] public virtual ICollection<Ban> BansReceived { get; set; }

        [InverseProperty(nameof(Ban.Moderator))]
        public virtual ICollection<Ban> BansGiven { get; set; }

        #endregion
    }
}
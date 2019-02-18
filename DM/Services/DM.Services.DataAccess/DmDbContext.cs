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

namespace DM.Services.DataAccess
{
    public class DmDbContext : DbContext
    {
        public DmDbContext(DbContextOptions options) : base(options)
        {
        }

        #region Users

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<Token> Tokens { get; set; }

        #endregion

        #region Common

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<TagGroup> TagGroups { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Upload> Uploads { get; set; }

        #endregion

        #region Forum

        public DbSet<Forum> Fora { get; set; }
        public DbSet<ForumTopic> ForumTopics { get; set; }
        public DbSet<ForumModerator> ForumModerators { get; set; }

        #endregion

        #region Games

        public DbSet<Game> Games { get; set; }
        public DbSet<GameTag> GameTags { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<BlackListLink> BlackListLinks { get; set; }
        public DbSet<CharacterRoomLink> CharacterRoomLinks { get; set; }

        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterAttribute> CharacterAttributes { get; set; }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAnticipation> PostAnticipations { get; set; }
        public DbSet<Vote> Votes { get; set; }

        #endregion

        #region Messaging

        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<UserConversationLink> UserConversationLinks { get; set; }
        public DbSet<Message> Messages { get; set; }

        #endregion

        #region Administration

        public DbSet<Report> Reports { get; set; }
        public DbSet<Warning> Warnings { get; set; }
        public DbSet<Ban> Bans { get; set; }

        #endregion
    }

    public class ReadDmDbContext : DmDbContext
    {
        public ReadDmDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Output
{
    /// <summary>
    /// DTO model for game
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Game identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// Game status
        /// </summary>
        public GameStatus Status { get; set; }

        /// <summary>
        /// Date the game was first released
        /// </summary>
        public DateTimeOffset? ReleaseDate { get; set; }

        /// <summary>
        /// Game master
        /// </summary>
        public GeneralUser Master { get; set; }

        /// <summary>
        /// Game master's assistant
        /// </summary>
        public GeneralUser Assistant { get; set; }

        /// <summary>
        /// Game premoderation moderator
        /// </summary>
        public GeneralUser Nanny { get; set; }

        /// <summary>
        /// Pending assistant if any
        /// </summary>
        public GeneralUser PendingAssistant { get; set; }

        /// <summary>
        /// User participation flag
        /// </summary>
        public IEnumerable<Guid> ActiveCharacterUserIds { get; set; }

        /// <summary>
        /// Game title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Game RPG system
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Game RPG setting
        /// </summary>
        public string SettingName { get; set; }

        /// <summary>
        /// Commentaries access mode
        /// </summary>
        public CommentariesAccessMode CommentariesAccessMode { get; set; }

        /// <summary>
        /// Number of unread game posts
        /// </summary>
        public int UnreadPostsCount { get; set; }

        /// <summary>
        /// Number of unread game commentaries
        /// </summary>
        public int UnreadCommentsCount { get; set; }

        /// <summary>
        /// Number of unread characters in game
        /// </summary>
        public int UnreadCharactersCount { get; set; }
    }
}
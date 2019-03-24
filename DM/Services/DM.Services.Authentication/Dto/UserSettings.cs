using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Authentication.Dto
{
    /// <summary>
    /// User settings
    /// </summary>
    public class UserSettings
    {
        /// <summary>
        /// Settings id (should be same as UserId)
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// If the user is nanny, each newbie linked to it will receive the custom private message upon registration
        /// </summary>
        public string NannyGreetingsMessage { get; set; }
        /// <summary>
        /// Color scheme for the website view
        /// </summary>
        public ColorScheme ColorScheme { get; set; }

        /// <summary>
        /// Number of game posts to display on a page
        /// </summary>
        public int PostsPerPage { get; set; }
        /// <summary>
        /// Number of game or topic comments to display on a page
        /// </summary>
        public int CommentsPerPage { get; set; }
        /// <summary>
        /// Number of forum topics to display on a page
        /// </summary>
        public int TopicsPerPage { get; set; }
        /// <summary>
        /// Number of private dialogues and messages to display on a page
        /// </summary>
        public int MessagesPerPage { get; set; }

        /// <summary>
        /// Default user settings for a guest or a newbie
        /// </summary>
        public static readonly UserSettings Default = new UserSettings
        {
            TopicsPerPage = 10,
            CommentsPerPage = 10,
            PostsPerPage = 10,
            MessagesPerPage = 10,
            ColorScheme = ColorScheme.Modern
        };
    }
}
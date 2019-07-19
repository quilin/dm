using DM.Services.Core.Dto.Enums;

namespace DM.Web.API.Dto.Games
{
    /// <summary>
    /// DTO model for game room
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Room identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Room title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Room access type
        /// </summary>
        public RoomAccessType Access { get; set; }

        /// <summary>
        /// Room content type
        /// </summary>
        public RoomType Type { get; set; }

        /// <summary>
        /// Number of unread posts
        /// </summary>
        public int UnreadPostsCount { get; set; }
    }
}
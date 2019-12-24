using System;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.Dto.Internal
{
    /// <summary>
    /// Internal DTO for post updating
    /// </summary>
    public class PostToUpdate : Post
    {
        /// <summary>
        /// Post room
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// Character identifier
        /// </summary>
        public Guid? CharacterId { get; set; }
    }
}
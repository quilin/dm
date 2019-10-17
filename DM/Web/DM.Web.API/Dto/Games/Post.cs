using System;
using System.Collections.Generic;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Web.API.Dto.Games
{
    /// <summary>
    /// DTO model for game post
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Post identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Parent room
        /// </summary>
        public Room Room { get; set; }

        /// <summary>
        /// Post character
        /// </summary>
        public Character Character { get; set; }

        /// <summary>
        /// Post author
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// Creation moment
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Last update moment
        /// </summary>
        public DateTimeOffset? Updated { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Additional text
        /// </summary>
        public string Commentary { get; set; }

        /// <summary>
        /// Private text to master
        /// </summary>
        public string MasterMessage { get; set; }

        /// <summary>
        /// Dice roll results
        /// </summary>
        public IEnumerable<DiceRoll> DiceRolls { get; set; }
    }
}
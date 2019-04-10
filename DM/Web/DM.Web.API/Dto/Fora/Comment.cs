using System;
using System.Collections.Generic;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Fora
{
    /// <summary>
    /// API DTO model for commentary
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Commentary identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// Creation moment
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Last update moment
        /// </summary>
        public DateTime? Updated { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Users who liked it
        /// </summary>
        public IEnumerable<User> Likes { get; set; }
    }
}
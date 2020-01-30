using System;
using System.Collections.Generic;

namespace DM.Web.API.Dto.Users
{
    /// <summary>
    /// DTO model for user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// Profile picture URL
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// User defined status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Rating
        /// </summary>
        public Rating Rating { get; set; }

        /// <summary>
        /// Last seen online moment
        /// </summary>
        public DateTimeOffset? Online { get; set; }

        /// <summary>
        /// User real name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User real location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// User ICQ number
        /// </summary>
        public string Icq { get; set; }

        /// <summary>
        /// User Skype login
        /// </summary>
        public string Skype { get; set; }

        /// <summary>
        /// User-defined extended information
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// User settings
        /// </summary>
        public UserSettings Settings { get; set; }
    }

    /// <summary>
    /// DTO model for user rating
    /// </summary>
    public class Rating
    {
        /// <summary>
        /// Rating participation flag
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Quality rating
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// Quantity rating
        /// </summary>
        public int Quantity { get; set; }
    }
}
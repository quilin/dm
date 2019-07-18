using System;

namespace DM.Services.Community.Dto
{
    /// <summary>
    /// DTO model for user additional data
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// User Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Date of user registration
        /// </summary>
        public DateTimeOffset RegistrationDate { get; set; }

        /// <summary>
        /// User-defined status
        /// </summary>
        public string Status { get; set; }

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
        /// User Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User-defined extended information
        /// </summary>
        public string Info { get; set; }
    }
}
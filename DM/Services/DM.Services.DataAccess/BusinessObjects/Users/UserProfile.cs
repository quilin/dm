using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;

namespace DM.Services.DataAccess.BusinessObjects.Users
{
    /// <summary>
    /// DAL model for user profile information
    /// </summary>
    [Table("UserDatas")]
    public class UserProfile : IRemovable
    {
        /// <summary>
        /// Profile identifier
        /// </summary>
        [Key]
        public Guid UserProfileId { get; set; }

        /// <summary>
        /// User identifier
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Custom status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Real name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Real location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// ICQ number
        /// </summary>
        public string Icq { get; set; }

        /// <summary>
        /// Skype name
        /// </summary>
        public string Skype { get; set; }

        /// <summary>
        /// Email display flag
        /// </summary>
        public bool ShowEmail { get; set; }

        /// <summary>
        /// Full user information
        /// </summary>
        public string Info { get; set; }

        /// <inheritdoc />
        public bool IsRemoved { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
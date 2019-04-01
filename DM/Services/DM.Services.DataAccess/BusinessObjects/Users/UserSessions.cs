using System;
using System.Collections.Generic;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.DataAccess.BusinessObjects.Users
{
    /// <summary>
    /// DAL model for user authentication
    /// </summary>
    [MongoCollectionName("UserSessions")]
    public class UserSessions
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Authentication sessions
        /// </summary>
        public List<Session> Sessions { get; set; }
    }

    /// <summary>
    /// DAL model for authentication session
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Session identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Expiration moment
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Persistence flag
        /// </summary>
        public bool IsPersistent { get; set; }
    }
}
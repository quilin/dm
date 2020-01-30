using System;

namespace DM.Services.Authentication.Dto
{
    /// <summary>
    /// DTO model for user authentication session
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Session persistence flag
        /// </summary>
        public bool IsPersistent { get; set; }

        /// <summary>
        /// Expiration date
        /// </summary>
        public DateTimeOffset ExpirationDate { get; set; }
    }
}
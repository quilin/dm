using System;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.DataAccess.BusinessObjects.Common
{
    /// <summary>
    /// DAL model for general chat message
    /// </summary>
    [MongoCollectionName("ChatMessages")]
    public class ChatMessage
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Author identifier
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Creation moment in ticks
        /// </summary>
        public DateTimeOffset CreateTicks { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }
    }
}
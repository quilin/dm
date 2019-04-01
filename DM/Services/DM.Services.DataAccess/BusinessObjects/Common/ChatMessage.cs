using System;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Bson.Serialization.Attributes;

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
        public long CreateTicks { get; set; }

        /// <summary>
        /// Creation moment
        /// </summary>
        [BsonIgnore]
        public DateTime CreateDate => DateTime.SpecifyKind(new DateTime(CreateTicks), DateTimeKind.Utc);

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }
    }
}
using System;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Bson.Serialization.Attributes;

namespace DM.Services.DataAccess.BusinessObjects.Common
{
    [MongoCollectionName("UnreadCounters")]
    [BsonIgnoreExtraElements]
    public class UnreadCounter
    {
        public Guid UserId { get; set; }
        public Guid EntityId { get; set; }
        public Guid ParentId { get; set; }
        public DateTime LastRead { get; set; }
        public UnreadEntryType EntryType { get; set; }
        public int Counter { get; set; }
    }
}
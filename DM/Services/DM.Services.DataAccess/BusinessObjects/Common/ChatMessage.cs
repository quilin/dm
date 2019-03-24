using System;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Bson.Serialization.Attributes;

namespace DM.Services.DataAccess.BusinessObjects.Common
{
    [MongoCollectionName("ChatMessages")]
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public long CreateTicks { get; set; }

        [BsonIgnore] public DateTime CreateDate => DateTime.SpecifyKind(new DateTime(CreateTicks), DateTimeKind.Utc);

        public string Text { get; set; }
    }
}
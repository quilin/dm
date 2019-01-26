using System;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Bson.Serialization.Attributes;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes
{
    [MongoCollectionName("AttributeSchemes")]
    [BsonKnownTypes(
        typeof(NumberAttributeConstraints),
        typeof(StringAttributeConstraints),
        typeof(ListAttributeConstraints))]
    public class AttributeScheme
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AttributeSpecification[] Specifications { get; set; }
    }
}
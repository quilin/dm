using System;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Bson.Serialization.Attributes;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes
{
    /// <summary>
    /// DAL model for attribute scheme
    /// </summary>
    [MongoCollectionName("AttributeSchemes")]
    [BsonKnownTypes(
        typeof(NumberAttributeConstraints),
        typeof(StringAttributeConstraints),
        typeof(ListAttributeConstraints))]
    public class AttributeScheme
    {
        /// <summary>
        /// Scheme identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Attribute specifications
        /// </summary>
        public AttributeSpecification[] Specifications { get; set; }
    }
}
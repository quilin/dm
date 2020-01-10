using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Bson.Serialization.Attributes;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes
{
    /// <summary>
    /// DAL model for attribute schema
    /// </summary>
    [MongoCollectionName("AttributeSchemas")]
    [BsonKnownTypes(
        typeof(NumberAttributeConstraints),
        typeof(StringAttributeConstraints),
        typeof(ListAttributeConstraints))]
    public class AttributeSchema : IRemovable
    {
        /// <summary>
        /// Schema identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Schema author and owner
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Schema access type
        /// </summary>
        public SchemaType Type { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Attribute specifications
        /// </summary>
        public IEnumerable<AttributeSpecification> Specifications { get; set; }

        /// <inheritdoc />
        public bool IsRemoved { get; set; }
    }
}
using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;

namespace DM.Services.Gaming.Dto.Output
{
    /// <summary>
    /// DTO model for game attribute schema
    /// </summary>
    public class AttributeSchema
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// User identifier
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Access type
        /// </summary>
        public SchemaType Type { get; set; }

        /// <summary>
        /// Specifications
        /// </summary>
        public IEnumerable<AttributeSpecification> Specifications { get; set; }
    }
}
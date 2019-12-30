using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Attributes;

namespace DM.Services.Gaming.Dto.Input
{
    /// <summary>
    /// DTO model for attribute schema updating
    /// </summary>
    public class UpdateAttributeSchema
    {
        /// <summary>
        /// Attribute schema identifier
        /// </summary>
        public Guid SchemaId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifications
        /// </summary>
        public IEnumerable<AttributeSpecification> Specifications { get; set; }
    }
}
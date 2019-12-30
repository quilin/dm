using System.Collections.Generic;
using DM.Services.Core.Dto.Attributes;

namespace DM.Services.Gaming.Dto.Input
{
    /// <summary>
    /// DTO model for attribute schema creating
    /// </summary>
    public class CreateAttributeSchema
    {
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
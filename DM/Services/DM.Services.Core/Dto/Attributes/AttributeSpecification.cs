using System;

namespace DM.Services.Core.Dto.Attributes
{
    /// <summary>
    /// DAL model for attribute specification
    /// </summary>
    public class AttributeSpecification
    {
        /// <summary>
        /// Specification identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specification constraints
        /// </summary>
        public AttributeConstraints Constraints { get; set; }
    }
}
using System;
using System.Linq;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Shared
{
    /// <inheritdoc />
    public class AttributeValueValidator : IAttributeValueValidator
    {
        /// <inheritdoc />
        public bool Validate(string value, AttributeSpecification specification)
        {
            if (value == null)
            {
                return false;
            }

            var trimmedValue = value.Trim();

            if (string.IsNullOrEmpty(trimmedValue) && specification.Required)
            {
                return false;
            }

            switch (specification.Type)
            {
                case AttributeSpecificationType.Number:
                    return int.TryParse(trimmedValue, out var numberValue) &&
                        (!specification.MaxValue.HasValue || numberValue <= specification.MaxValue.Value) &&
                        (!specification.MinValue.HasValue || numberValue >= specification.MinValue.Value);
                case AttributeSpecificationType.String:
                    return specification.MaxLength.HasValue && specification.MaxLength.Value >= trimmedValue.Length;
                case AttributeSpecificationType.List:
                    return specification.Values != null && specification.Values.Any(v => v.Value == trimmedValue);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
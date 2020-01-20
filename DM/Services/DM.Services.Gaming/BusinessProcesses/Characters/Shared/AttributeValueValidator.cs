using System.Linq;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Shared
{
    /// <inheritdoc />
    public class AttributeValueValidator : IAttributeValueValidator
    {
        /// <inheritdoc />
        public (bool valid, string error) Validate(string value, AttributeSpecification specification)
        {
            if (value == null)
            {
                return (false, ValidationError.Empty);
            }

            var trimmedValue = value.Trim();

            if (string.IsNullOrEmpty(trimmedValue) && specification.Required)
            {
                return (false, ValidationError.Empty);
            }

            switch (specification.Type)
            {
                case AttributeSpecificationType.Number when
                    !int.TryParse(trimmedValue, out var numberValue) ||
                        specification.MaxValue.HasValue && numberValue > specification.MaxValue.Value ||
                        specification.MinValue.HasValue && numberValue < specification.MinValue.Value:
                    return (false, ValidationError.Invalid);
                case AttributeSpecificationType.String when
                    !specification.MaxLength.HasValue || trimmedValue.Length > specification.MaxLength.Value:
                    return (false, ValidationError.Long);
                case AttributeSpecificationType.List when
                    specification.Values != null && specification.Values.All(v => v.Value != trimmedValue):
                    return (false, ValidationError.Invalid);
                default:
                    return (true, null);
            }
        }
    }
}
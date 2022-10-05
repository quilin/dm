using System.Linq;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Shared;

/// <inheritdoc />
internal class AttributeValueValidator : IAttributeValueValidator
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
            return (false, AttributeValidationError.RequiredMissing);
        }

        switch (specification.Type)
        {
            case AttributeSpecificationType.Number:
                if (!int.TryParse(trimmedValue, out var numberValue))
                {
                    return (false, AttributeValidationError.NotANumber);
                }

                if (specification.MaxValue.HasValue && numberValue > specification.MaxValue.Value ||
                    specification.MinValue.HasValue && numberValue < specification.MinValue.Value)
                {
                    return (false, AttributeValidationError.NumberNotInRange(
                        specification.MinValue, specification.MaxValue));
                }

                break;
            case AttributeSpecificationType.String when specification.MaxLength.HasValue:
                if (specification.MaxLength.Value < trimmedValue.Length)
                {
                    return (false, AttributeValidationError.StringTooLong(specification.MaxLength.Value));
                }

                break;
            case AttributeSpecificationType.List when specification.Values != null:
                if (specification.Values.All(v => v.Value != trimmedValue))
                {
                    return (false,
                        AttributeValidationError.NotPresentInList(specification.Values.Select(v => v.Value)));
                }

                break;
            default:
                return (false, ValidationError.Invalid);
        }

        return (true, null);
    }
}
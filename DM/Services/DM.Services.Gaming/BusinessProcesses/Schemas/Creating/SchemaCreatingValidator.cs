using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Shared;
using FluentValidation;
using FluentValidation.Results;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating;

/// <inheritdoc />
internal class SchemaCreatingValidator : ISchemaCreatingValidator
{
    /// <inheritdoc />
    public void ValidateAndThrow(AttributeSchema attributeSchema)
    {
        var errors = new List<ValidationFailure>();
        if (string.IsNullOrEmpty(attributeSchema.Title))
        {
            errors.Add(new ValidationFailure(nameof(AttributeSchema.Title), ValidationError.Empty));
        }
        else if (attributeSchema.Title.Trim().Length > 50)
        {
            errors.Add(new ValidationFailure(nameof(AttributeSchema.Title), ValidationError.Long));
        }

        var counter = 0;
        var specificationNames = new HashSet<string>();
        foreach (var specification in attributeSchema.Specifications)
        {
            var propertyNamePart = $"{nameof(AttributeSchema.Specifications)}.[{counter}]";
            var title = specification.Title?.Trim();
            if (string.IsNullOrEmpty(title))
            {
                errors.Add(new ValidationFailure($"{propertyNamePart}.{nameof(AttributeSpecification.Title)}",
                    ValidationError.Empty));
            }
            else if (title.Length > 50)
            {
                errors.Add(new ValidationFailure($"{propertyNamePart}.{nameof(AttributeSpecification.Title)}",
                    ValidationError.Long));
            }
            else if (specificationNames.Contains(title))
            {
                errors.Add(new ValidationFailure($"{propertyNamePart}.{nameof(AttributeSpecification.Title)}",
                    ValidationError.Taken));
            }
            else
            {
                specificationNames.Add(title);
            }

            switch (specification.Type)
            {
                case AttributeSpecificationType.Number:
                    if (specification.MaxValue.HasValue && specification.MinValue.HasValue &&
                        specification.MaxValue.Value < specification.MinValue.Value)
                    {
                        errors.Add(new ValidationFailure(
                            $"{propertyNamePart}.{nameof(AttributeSpecification.MaxValue)}",
                            ValidationError.Invalid));
                    }

                    break;

                case AttributeSpecificationType.String:
                    if (!specification.MaxLength.HasValue)
                    {
                        errors.Add(new ValidationFailure(
                            $"{propertyNamePart}.{nameof(AttributeSpecification.MaxLength)}",
                            ValidationError.Empty));
                    }
                    else if (specification.MaxLength <= 0)
                    {
                        errors.Add(new ValidationFailure(
                            $"{propertyNamePart}.{nameof(AttributeSpecification.MaxLength)}",
                            ValidationError.Invalid));
                    }

                    break;

                case AttributeSpecificationType.List:
                    var constraintsNamePart = $"{propertyNamePart}.{nameof(AttributeSpecification.Values)}";
                    var values = new HashSet<string>();
                    var valueCounter = 0;
                    if (specification.Values == null || !specification.Values.Any())
                    {
                        errors.Add(new ValidationFailure(constraintsNamePart, ValidationError.Empty));
                        break;
                    }
                        
                    foreach (var listValue in specification.Values)
                    {
                        var valueName = $"{constraintsNamePart}.[{valueCounter}].{nameof(ListValue.Value)}";
                        var value = listValue.Value?.Trim();
                        if (string.IsNullOrEmpty(value))
                        {
                            errors.Add(new ValidationFailure(valueName, ValidationError.Empty));
                        }
                        else if (value.Length > 50)
                        {
                            errors.Add(new ValidationFailure(valueName, ValidationError.Long));
                        }
                        else if (values.Contains(value))
                        {
                            errors.Add(new ValidationFailure(valueName, ValidationError.Taken));
                        }
                        else
                        {
                            values.Add(value);
                        }

                        valueCounter++;
                    }

                    break;

                default:
                    errors.Add(new ValidationFailure(propertyNamePart, ValidationError.Invalid));
                    break;
            }

            counter++;
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}
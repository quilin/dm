using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.BusinessProcesses.Characters.Shared;
using DM.Services.Gaming.Dto.Shared;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input;

/// <inheritdoc />
internal class UpdateCharacterValidator : AbstractValidator<UpdateCharacter>
{
    private const string ErrorMessage = nameof(ErrorMessage);
    private const string SchemaCacheKey = nameof(SchemaCacheKey);

    /// <inheritdoc />
    public UpdateCharacterValidator(
        ICharacterValidationRepository validationRepository,
        IAttributeValueValidator attributeValueValidator)
    {
        When(c => c.Name != default, () =>
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MaximumLength(50).WithMessage(ValidationError.Long));

        When(c => c.Race != default, () =>
            RuleFor(c => c.Race)
                .MaximumLength(30).WithMessage(ValidationError.Long));

        When(c => c.Race != default, () =>
            RuleFor(c => c.Class)
                .MaximumLength(30).WithMessage(ValidationError.Long));

        When(c => c.Attributes != null && c.Attributes.Any(), () =>
            RuleForEach(c => c.Attributes)
                .MustAsync(async (c, attribute, context, _) =>
                {
                    if (!context.RootContextData.TryGetValue(SchemaCacheKey, out var schemaWrapper) ||
                        schemaWrapper is not Dictionary<Guid, AttributeSpecification> specifications)
                    {
                        var schema = await validationRepository.GetCharacterSchema(c.CharacterId);
                        specifications = schema.Specifications.ToDictionary(s => s.Id);
                        context.RootContextData[SchemaCacheKey] = specifications;
                    }

                    if (!specifications.TryGetValue(attribute.Id, out var specification))
                    {
                        context.MessageFormatter.AppendArgument(ErrorMessage,
                            AttributeValidationError.InvalidSpecification);
                        return false;
                    }

                    var (valid, error) = attributeValueValidator.Validate(attribute.Value, specification);
                    context.MessageFormatter.AppendArgument(ErrorMessage, error);
                    return valid;
                })
                .WithMessage($"{{{ErrorMessage}}}"));
    }
}
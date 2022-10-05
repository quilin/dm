using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.BusinessProcesses.Characters.Shared;
using DM.Services.Gaming.Dto.Shared;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input;

/// <summary>
/// Validator for character creation DTO
/// </summary>
internal class CreateCharacterValidator : AbstractValidator<CreateCharacter>
{
    private const string SchemaCacheKey = nameof(SchemaCacheKey);
    private const string ErrorMessage = nameof(ErrorMessage);

    /// <inheritdoc />
    public CreateCharacterValidator(
        ICharacterValidationRepository validationRepository,
        IAttributeValueValidator attributeValueValidator)
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MaximumLength(50).WithMessage(ValidationError.Long);

        RuleFor(c => c.Race)
            .MaximumLength(30).WithMessage(ValidationError.Long);

        RuleFor(c => c.Class)
            .MaximumLength(30).WithMessage(ValidationError.Long);

        WhenAsync(validationRepository.GameRequiresAttributes, () =>
        {
            RuleFor(c => c.Attributes)
                .MustAsync(async (c, _, context, _) =>
                {
                    if (!context.RootContextData.TryGetValue(SchemaCacheKey, out var schemaWrapper) ||
                        schemaWrapper is not Dictionary<Guid, AttributeSpecification> specifications)
                    {
                        var schema = await validationRepository.GetGameSchema(c.GameId);
                        specifications = schema.Specifications.ToDictionary(s => s.Id);
                        context.RootContextData[SchemaCacheKey] = specifications;
                    }

                    var attributeIndex = c.Attributes.ToDictionary(a => a.Id);
                    var missingAttributes = specifications
                        .Where(s => s.Value.Required && !attributeIndex.ContainsKey(s.Key))
                        .Select(s => (s.Value.Id, s.Value.Title))
                        .ToArray();
                    context.MessageFormatter.AppendArgument(ErrorMessage,
                        AttributeValidationError.ManyRequiredMissing(missingAttributes));
                    return !missingAttributes.Any();
                })
                .WithMessage($"{{{ErrorMessage}}}");

            RuleForEach(c => c.Attributes)
                .MustAsync(async (c, attribute, context, _) =>
                {
                    if (!context.RootContextData.TryGetValue(SchemaCacheKey, out var schemaWrapper) ||
                        schemaWrapper is not Dictionary<Guid, AttributeSpecification> specifications)
                    {
                        var schema = await validationRepository.GetGameSchema(c.GameId);
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
                .WithMessage($"{{{ErrorMessage}}}");
        });
    }
}
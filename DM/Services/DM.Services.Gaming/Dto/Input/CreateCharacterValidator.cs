using System.Linq;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.BusinessProcesses.Characters.Shared;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace DM.Services.Gaming.Dto.Input
{
    /// <summary>
    /// Validator for character creation DTO
    /// </summary>
    public class CreateCharacterValidator : AbstractValidator<CreateCharacter>
    {
        private const string ValidationCacheKey = nameof(ValidationCacheKey);
        private const string AttributeValidationArgumentName = nameof(AttributeValidationArgumentName);

        /// <inheritdoc />
        public CreateCharacterValidator(
            IMemoryCache memoryCache,
            IGameReadingService gameReadingService,
            ISchemaReadingService schemaReadingService,
            IAttributeValueValidator attributeValueValidator)
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MaximumLength(50).WithMessage(ValidationError.Long);

            RuleFor(c => c.Race)
                .MaximumLength(30).WithMessage(ValidationError.Long);

            RuleFor(c => c.Class)
                .MaximumLength(30).WithMessage(ValidationError.Long);

            RuleForEach(c => c.Attributes)
                .MustAsync(async (model, attribute, context, cancellationToken) =>
                {
                    var specifications = await memoryCache.GetOrCreateAsync(ValidationCacheKey, async _ =>
                    {
                        var game = await gameReadingService.GetGame(model.GameId);
                        if (!game.AttributeSchemaId.HasValue)
                        {
                            return null;
                        }

                        var schema = await schemaReadingService.Get(game.AttributeSchemaId.Value);
                        return schema.Specifications.ToDictionary(s => s.Id);
                    });

                    if (specifications == null)
                    {
                        context.MessageFormatter.AppendArgument(AttributeValidationArgumentName, "Game has no schema");
                        return false;
                    }

                    if (!specifications.TryGetValue(attribute.Id, out var specification))
                    {
                        context.MessageFormatter.AppendArgument(AttributeValidationArgumentName,
                            "Invalid specification");
                        return false;
                    }

                    var (valid, error) = attributeValueValidator.Validate(attribute.Value, specification);
                    if (!valid)
                    {
                        context.MessageFormatter.AppendArgument(AttributeValidationArgumentName, error);
                    }

                    return valid;
                })
                .WithMessage($"{{{AttributeValidationArgumentName}}}");
        }
    }
}
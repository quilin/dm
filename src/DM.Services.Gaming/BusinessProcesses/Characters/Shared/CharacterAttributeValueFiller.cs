using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Output;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Shared;

/// <inheritdoc />
internal class CharacterAttributeValueFiller : ICharacterAttributeValueFiller
{
    private readonly ISchemaReadingService schemaReadingService;
    private readonly IAttributeValueValidator attributeValueValidator;

    /// <inheritdoc />
    public CharacterAttributeValueFiller(
        ISchemaReadingService schemaReadingService,
        IAttributeValueValidator attributeValueValidator)
    {
        this.schemaReadingService = schemaReadingService;
        this.attributeValueValidator = attributeValueValidator;
    }

    /// <inheritdoc />
    public async Task Fill(IEnumerable<Character> characters, Guid? schemaId)
    {
        if (!schemaId.HasValue)
        {
            foreach (var character in characters)
            {
                character.Attributes = null;
            }

            return;
        }

        var schema = await schemaReadingService.Get(schemaId.Value);
        foreach (var character in characters)
        {
            var attributeIndex = character.Attributes.ToDictionary(a => a.Id);
            var filledAttributes = new List<CharacterAttribute>(character.Attributes.Count());
            foreach (var specification in schema.Specifications)
            {
                if (!attributeIndex.TryGetValue(specification.Id, out var attribute))
                {
                    filledAttributes.Add(new CharacterAttribute
                    {
                        Id = specification.Id,
                        Title = specification.Title,
                        Modifier = null,
                        Value = null,
                        Inconsistent = true
                    });
                }
                else
                {
                    var (valid, _) = attributeValueValidator.Validate(attribute.Value, specification);
                    var filledAttribute = new CharacterAttribute
                    {
                        Id = specification.Id,
                        Title = specification.Title,
                        Value = attribute.Value,
                        Inconsistent = !valid
                    };
                    if (specification.Type == AttributeSpecificationType.List)
                    {
                        var matchingValue = specification.Values.FirstOrDefault(v => v.Value == attribute.Value);
                        if (matchingValue != null)
                        {
                            filledAttribute.Modifier = matchingValue.Modifier;
                        }
                    }

                    filledAttributes.Add(filledAttribute);
                }
            }

            character.Attributes = filledAttributes;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Output;
using CharacterAttribute = DM.Services.Gaming.Dto.Output.CharacterAttribute;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Reading
{
    /// <summary>
    /// Extensions for character attribute values filling
    /// </summary>
    public static class CharacterAttributeFillExtensions
    {
        /// <summary>
        /// Fill attribute values for given characters
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="attributeSchemaId"></param>
        /// <param name="characters"></param>
        /// <returns></returns>
        public static async Task FillAttributeValues(this ISchemaReadingRepository repository,
            Guid? attributeSchemaId, IEnumerable<Character> characters)
        {
            if (!attributeSchemaId.HasValue)
            {
                foreach (var character in characters)
                {
                    character.Attributes = null;
                }

                return;
            }

            var schema = await repository.GetSchema(attributeSchemaId.Value);
            var specificationsIndex = schema.Specifications.ToDictionary(s => s.Id);

            foreach (var character in characters)
            {
                var filledAttributes = new List<CharacterAttribute>();
                foreach (var attribute in character.Attributes)
                {
                    if (!specificationsIndex.TryGetValue(attribute.Id, out var specification))
                    {
                        continue;
                    }

                    var filledAttribute = new CharacterAttribute
                    {
                        Id = attribute.Id,
                        Title = specification.Name,
                        Value = attribute.Value
                    };
                    if (specification.Constraints is ListAttributeConstraints listConstraints)
                    {
                        var matchingValue = listConstraints.Values.FirstOrDefault(v => v.Value == attribute.Value);
                        if (matchingValue != null)
                        {
                            filledAttribute.Modifier = matchingValue.Modifier;
                        }
                    }

                    filledAttributes.Add(filledAttribute);
                }

                character.Attributes = filledAttributes;
            }
        }
    }
}
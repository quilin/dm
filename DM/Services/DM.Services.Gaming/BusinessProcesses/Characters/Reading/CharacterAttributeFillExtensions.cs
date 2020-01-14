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

            foreach (var character in characters)
            {
                var attributesIndex = character.Attributes.ToDictionary(a => a.Id, a => a.Value);
                var filledAttributes = new List<CharacterAttribute>();

                foreach (var specification in schema.Specifications)
                {
                    var filledAttribute = new CharacterAttribute
                    {
                        Id = specification.Id,
                        Title = specification.Title
                    };
                    filledAttributes.Add(filledAttribute);

                    if (!attributesIndex.TryGetValue(specification.Id, out var value))
                    {
                        continue;
                    }

                    filledAttribute.Value = value;

                    if (!(specification.Constraints is ListAttributeConstraints listConstraints))
                    {
                        continue;
                    }

                    var matchingValue = listConstraints.Values.FirstOrDefault(v => v.Value == value);
                    if (matchingValue != null)
                    {
                        filledAttribute.Modifier = matchingValue.Modifier;
                    }
                }

                character.Attributes = filledAttributes;
            }
        }
    }
}
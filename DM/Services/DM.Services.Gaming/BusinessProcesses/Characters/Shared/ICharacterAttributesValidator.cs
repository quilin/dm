using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Shared
{
    /// <summary>
    /// Validator or incoming 
    /// </summary>
    public interface ICharacterAttributesValidator
    {
        /// <summary>
        /// Validate incoming attributes against given schema
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="attributeSchemaId"></param>
        /// <returns></returns>
        IDictionary<Guid, string> ValidateAndThrow(IEnumerable<CharacterAttribute> attributes,
            Guid? attributeSchemaId);
    }

    /// <inheritdoc />
    public class CharacterAttributesValidator : ICharacterAttributesValidator
    {
        private readonly ISchemaReadingRepository schemaReadingRepository;

        /// <inheritdoc />
        public CharacterAttributesValidator(
            ISchemaReadingRepository schemaReadingRepository)
        {
            this.schemaReadingRepository = schemaReadingRepository;
        }
        
        /// <inheritdoc />
        public IDictionary<Guid, string> ValidateAndThrow(IEnumerable<CharacterAttribute> attributes, Guid? attributeSchemaId)
        {
            if (!attributeSchemaId.HasValue)
            {
                return null;
            }
        }
    }
}
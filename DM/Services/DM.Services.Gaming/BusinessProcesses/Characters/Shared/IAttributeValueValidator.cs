using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Shared
{
    /// <summary>
    /// Validator for character attribute value
    /// </summary>
    public interface IAttributeValueValidator
    {
        /// <summary>
        /// Validate value against specification
        /// </summary>
        /// <param name="value"></param>
        /// <param name="specification"></param>
        /// <returns></returns>
        bool Validate(string value, AttributeSpecification specification);
    }
}
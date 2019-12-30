using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Updating
{
    /// <summary>
    /// Service for attribute schema updating
    /// </summary>
    public interface ISchemaUpdatingService
    {
        /// <summary>
        /// Update attribute schema
        /// </summary>
        /// <param name="updateAttributeSchema">DTO for updating</param>
        /// <returns></returns>
        Task<AttributeSchema> Update(UpdateAttributeSchema updateAttributeSchema);
    }
}
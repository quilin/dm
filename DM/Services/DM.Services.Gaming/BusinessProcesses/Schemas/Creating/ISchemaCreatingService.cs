using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating
{
    /// <summary>
    /// Service for attribute schema creating
    /// </summary>
    public interface ISchemaCreatingService
    {
        /// <summary>
        /// Create new attribute schema
        /// </summary>
        /// <param name="createAttributeSchema">DTO for creating</param>
        /// <returns></returns>
        Task<AttributeSchema> Create(CreateAttributeSchema createAttributeSchema);
    }
}
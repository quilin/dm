using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating;

/// <summary>
/// Service for attribute schema creating
/// </summary>
public interface ISchemaCreatingService
{
    /// <summary>
    /// Create new attribute schema
    /// </summary>
    /// <param name="attributeSchema">DTO for creating</param>
    /// <returns></returns>
    Task<AttributeSchema> Create(AttributeSchema attributeSchema);
}
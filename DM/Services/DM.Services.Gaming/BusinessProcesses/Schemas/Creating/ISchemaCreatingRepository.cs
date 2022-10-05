using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Shared;
using DbAttributeSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating;

/// <summary>
/// Storage for attribute schema creating
/// </summary>
internal interface ISchemaCreatingRepository
{
    /// <summary>
    /// Create new attribute schema
    /// </summary>
    /// <returns></returns>
    Task<AttributeSchema> Create(DbAttributeSchema schema);
}
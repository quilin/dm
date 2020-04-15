using System.Threading.Tasks;
using DbAttributeSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating
{
    /// <summary>
    /// Storage for attribute schema creating
    /// </summary>
    public interface ISchemaCreatingRepository
    {
        /// <summary>
        /// Create new attribute schema
        /// </summary>
        /// <returns></returns>
        Task<DbAttributeSchema> Create(DbAttributeSchema schema);
    }
}
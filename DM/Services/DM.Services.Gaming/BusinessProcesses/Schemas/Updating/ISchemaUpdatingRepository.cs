using System.Threading.Tasks;
using DbAttributeSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Updating
{
    /// <summary>
    /// Storage for attribute schema updating
    /// </summary>
    public interface ISchemaUpdatingRepository
    {
        /// <summary>
        /// Update existing attribute schema
        /// </summary>
        /// <param name="schema">DAL model</param>
        /// <returns></returns>
        Task<DbAttributeSchema> UpdateSchema(DbAttributeSchema schema);
    }
}
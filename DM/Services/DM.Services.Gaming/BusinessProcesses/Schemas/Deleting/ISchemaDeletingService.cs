using System;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Deleting
{
    /// <summary>
    /// Service for attribute schema deleting
    /// </summary>
    public interface ISchemaDeletingService
    {
        /// <summary>
        /// Delete existing attribute schema
        /// </summary>
        /// <param name="attributeSchemaId">Attribute schema identifier</param>
        /// <returns></returns>
        Task Delete(Guid attributeSchemaId);
    }
}
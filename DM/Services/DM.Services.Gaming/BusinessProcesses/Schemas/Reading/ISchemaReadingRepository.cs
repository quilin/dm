using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Output;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Reading
{
    /// <summary>
    /// Storage for attribute schemas
    /// </summary>
    public interface ISchemaReadingRepository
    {
        /// <summary>
        /// Get list of available schemas
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        Task<IEnumerable<AttributeSchema>> Get(Guid userId);

        /// <summary>
        /// Get certain attribute schema
        /// </summary>
        /// <param name="schemaId">Schema identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        Task<AttributeSchema> Get(Guid schemaId, Guid userId);
    }
}
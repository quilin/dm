using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;

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
        Task<IEnumerable<AttributeSchema>> GetSchemata(Guid userId);

        /// <summary>
        /// Get certain attribute schema
        /// </summary>
        /// <param name="schemaId">Schema identifier</param>
        /// <returns></returns>
        Task<AttributeSchema> GetSchema(Guid schemaId);

        /// <summary>
        /// Get list of users by identifiers
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<IEnumerable<GeneralUser>> GetSchemataAuthors(ICollection<Guid> userIds);
    }
}
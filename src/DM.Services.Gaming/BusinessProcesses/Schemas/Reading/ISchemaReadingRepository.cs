using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Reading;

/// <summary>
/// Storage for attribute schemas
/// </summary>
internal interface ISchemaReadingRepository
{
    /// <summary>
    /// Get list of available schemas
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<AttributeSchema>> GetSchemata(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Get certain attribute schema
    /// </summary>
    /// <param name="schemaId">Schema identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AttributeSchema> GetSchema(Guid schemaId, CancellationToken cancellationToken);
}
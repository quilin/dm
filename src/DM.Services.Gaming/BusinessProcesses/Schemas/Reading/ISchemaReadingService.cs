using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Reading;

/// <summary>
/// Service for reading attribute schemas
/// </summary>
public interface ISchemaReadingService
{
    /// <summary>
    /// Get list of available schemas
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<AttributeSchema>> Get(CancellationToken cancellationToken);

    /// <summary>
    /// Get certain attribute schema
    /// </summary>
    /// <param name="schemaId">Schema identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AttributeSchema> Get(Guid schemaId, CancellationToken cancellationToken);
}
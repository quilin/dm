using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Deleting;

/// <summary>
/// Service for attribute schema deleting
/// </summary>
public interface ISchemaDeletingService
{
    /// <summary>
    /// Delete existing attribute schema
    /// </summary>
    /// <param name="schemaId">Attribute schema identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid schemaId, CancellationToken cancellationToken);
}
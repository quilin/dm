using System;
using System.Threading;
using System.Threading.Tasks;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Deleting;

/// <summary>
/// Storage for attribute schema deleting
/// </summary>
internal interface ISchemaDeletingRepository
{
    /// <summary>
    /// Delete existing attribute schema
    /// </summary>
    /// <param name="schemaId">Schema identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid schemaId, CancellationToken cancellationToken);
}
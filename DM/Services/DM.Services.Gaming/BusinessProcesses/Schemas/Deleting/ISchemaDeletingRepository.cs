using System;
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
    /// <returns></returns>
    Task Delete(Guid schemaId);
}
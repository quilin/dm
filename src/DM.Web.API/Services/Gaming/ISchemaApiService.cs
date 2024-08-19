using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games.Attributes;

namespace DM.Web.API.Services.Gaming;

/// <summary>
/// API service for attribute schemas
/// </summary>
public interface ISchemaApiService
{
    /// <summary>
    /// Get all available attribute schemas
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ListEnvelope<AttributeSchema>> Get(CancellationToken cancellationToken);

    /// <summary>
    /// Get single attribute schema
    /// </summary>
    /// <param name="schemaId">Schema identifier</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<AttributeSchema>> Get(Guid schemaId, CancellationToken cancellationToken);

    /// <summary>
    /// Create new attribute schema
    /// </summary>
    /// <param name="schema">Schema DTO</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<AttributeSchema>> Create(AttributeSchema schema, CancellationToken cancellationToken);

    /// <summary>
    /// Update existing attribute schema
    /// </summary>
    /// <param name="schemaId">Schema identifier</param>
    /// <param name="schema">Schema DTO</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Envelope<AttributeSchema>> Update(Guid schemaId, AttributeSchema schema,
        CancellationToken cancellationToken);

    /// <summary>
    /// Delete existing attribute schema
    /// </summary>
    /// <param name="schemaId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Delete(Guid schemaId, CancellationToken cancellationToken);
}
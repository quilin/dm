using System;
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
    /// <returns></returns>
    Task<ListEnvelope<AttributeSchema>> Get();

    /// <summary>
    /// Get single attribute schema
    /// </summary>
    /// <param name="schemaId">Schema identifier</param>
    /// <returns></returns>
    Task<Envelope<AttributeSchema>> Get(Guid schemaId);

    /// <summary>
    /// Create new attribute schema
    /// </summary>
    /// <param name="schema">Schema DTO</param>
    /// <returns></returns>
    Task<Envelope<AttributeSchema>> Create(AttributeSchema schema);

    /// <summary>
    /// Update existing attribute schema
    /// </summary>
    /// <param name="schemaId">Schema identifier</param>
    /// <param name="schema">Schema DTO</param>
    /// <returns></returns>
    Task<Envelope<AttributeSchema>> Update(Guid schemaId, AttributeSchema schema);

    /// <summary>
    /// Delete existing attribute schema
    /// </summary>
    /// <param name="schemaId"></param>
    /// <returns></returns>
    Task Delete(Guid schemaId);
}
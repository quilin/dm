using System.Collections.Generic;

namespace DM.Web.API.Dto.Contracts;

/// <summary>
/// Enveloped DTO model
/// </summary>
/// <typeparam name="T">Enveloped type</typeparam>
public class Envelope<T>
{
    /// <inheritdoc />
    public Envelope(T resource, object metadata = null)
    {
        Resource = resource;
        Metadata = metadata;
    }

    /// <summary>
    /// Enveloped resource
    /// </summary>
    public T Resource { get; }

    /// <summary>
    /// Additional metadata
    /// </summary>
    public object Metadata { get; }
}

/// <summary>
/// Enveloped list DTO model
/// </summary>
/// <typeparam name="T">Enveloped type</typeparam>
public class ListEnvelope<T>
{
    /// <inheritdoc />
    public ListEnvelope(IEnumerable<T> resources, Paging paging = null)
    {
        Resources = resources;
        Paging = paging;
    }

    /// <summary>
    /// Enveloped resources
    /// </summary>
    public IEnumerable<T> Resources { get; }

    /// <summary>
    /// Paging data
    /// </summary>
    public Paging Paging { get; }
}
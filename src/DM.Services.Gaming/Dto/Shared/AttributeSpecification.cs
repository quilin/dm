using System;
using System.Collections.Generic;

namespace DM.Services.Gaming.Dto.Shared;

/// <summary>
/// DTO model for attribute specification
/// </summary>
public class AttributeSpecification
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Specification type
    /// </summary>
    public AttributeSpecificationType Type { get; set; }

    /// <summary>
    /// Value is required
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Minimal value
    /// </summary>
    public int? MinValue { get; set; }

    /// <summary>
    /// Maximal value
    /// </summary>
    public int? MaxValue { get; set; }

    /// <summary>
    /// Maximal length
    /// </summary>
    public int? MaxLength { get; set; }

    /// <summary>
    /// Possible values
    /// </summary>
    public IEnumerable<ListValue> Values { get; set; }
}
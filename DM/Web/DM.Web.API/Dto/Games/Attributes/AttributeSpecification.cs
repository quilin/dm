using System;
using System.Collections.Generic;

namespace DM.Web.API.Dto.Games.Attributes;

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
    /// Value is required
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Constraints type
    /// </summary>
    public AttributeSpecificationType Type { get; set; }

    /// <summary>
    /// Minimal value for number constraints
    /// </summary>
    public int? MinValue { get; set; }

    /// <summary>
    /// Maximal value for number constraints
    /// </summary>
    public int? MaxValue { get; set; }

    /// <summary>
    /// Maximal length for string constraints
    /// </summary>
    public int? MaxLength { get; set; }

    /// <summary>
    /// List of possible values for list constraints
    /// </summary>
    public IEnumerable<AttributeValueSpecification> Values { get; set; }
}
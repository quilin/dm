using System;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;

/// <summary>
/// DAL model for attribute specification
/// </summary>
public class AttributeSpecification
{
    /// <summary>
    /// Specification identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Display name
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Specification constraints
    /// </summary>
    public AttributeConstraints Constraints { get; set; }
}
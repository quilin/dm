using MongoDB.Bson.Serialization.Attributes;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;

/// <summary>
/// Base DAL model for attribute constraints
/// </summary>
[BsonKnownTypes(
    typeof(NumberAttributeConstraints),
    typeof(StringAttributeConstraints),
    typeof(ListAttributeConstraints))]
public abstract class AttributeConstraints
{
    /// <summary>
    /// Attribute value requirement flag
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Get default attribute value
    /// </summary>
    /// <returns></returns>
    public abstract string GetDefaultValue();
}
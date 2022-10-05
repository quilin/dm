using System.Linq;
using AutoMapper;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;

namespace DM.Services.Gaming.Dto.Shared;

/// <inheritdoc />
internal class SpecificationConverter : ITypeConverter<
    DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSpecification, AttributeSpecification>
{
    /// <inheritdoc />
    public AttributeSpecification Convert(
        DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSpecification source,
        AttributeSpecification destination,
        ResolutionContext context)
    {
        var result = new AttributeSpecification
        {
            Id = source.Id,
            Title = source.Title,
            Type = ResolveType(source),
            Required = source.Constraints.Required
        };

        switch (source.Constraints)
        {
            case NumberAttributeConstraints numberConstraints:
                result.MinValue = numberConstraints.MinValue;
                result.MaxValue = numberConstraints.MaxValue;
                result.Values = null;
                return result;
            case StringAttributeConstraints stringConstraints:
                result.MaxLength = stringConstraints.MaxLength;
                result.Values = null;
                return result;
            case ListAttributeConstraints listConstraints:
                result.Values = listConstraints.Values.Select(v => new ListValue
                {
                    Value = v.Value,
                    Modifier = v.Modifier
                });
                return result;
            default:
                return result;
        }
    }

    private static AttributeSpecificationType ResolveType(
        DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSpecification source) =>
        source.Constraints switch
        {
            NumberAttributeConstraints _ => AttributeSpecificationType.Number,
            StringAttributeConstraints _ => AttributeSpecificationType.String,
            ListAttributeConstraints _ => AttributeSpecificationType.List,
            _ => AttributeSpecificationType.Number
        };
}
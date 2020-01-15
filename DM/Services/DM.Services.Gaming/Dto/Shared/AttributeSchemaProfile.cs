using System.Linq;
using AutoMapper;
using DbSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;
using DbSpecification = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSpecification;
using DbNumberConstraints = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.NumberAttributeConstraints;
using DbStringConstraints = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.StringAttributeConstraints;
using DbListConstraints = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.ListAttributeConstraints;
using DbListValue = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.ListAttributeValue;

namespace DM.Services.Gaming.Dto.Shared
{
    /// <inheritdoc />
    public class AttributeSchemaProfile : Profile
    {
        /// <inheritdoc />
        public AttributeSchemaProfile()
        {
            CreateMap<DbSchema, AttributeSchema>();
            CreateMap<DbSpecification, AttributeSpecification>();
                // .ConvertUsing<SpecificationConverter>();
            CreateMap<DbListValue, ListValue>();
        }
        
        private class SpecificationConverter : ITypeConverter<DbSpecification, AttributeSpecification>
        {
            public AttributeSpecification Convert(DbSpecification source, AttributeSpecification destination,
                ResolutionContext context)
            {
                var result = new AttributeSpecification
                {
                    Id = source.Id,
                    Title = source.Title,
                    Type = ResolveType(source),
                    Required = source.Constraints.Required
                };

                if (source.Constraints is DbNumberConstraints numberConstraints)
                {
                    result.MinValue = numberConstraints.MinValue;
                    result.MaxValue = numberConstraints.MaxValue;
                }

                if (source.Constraints is DbStringConstraints stringConstraints)
                {
                    result.MaxValue = stringConstraints.MaxLength;
                }

                if (source.Constraints is DbListConstraints listConstraints)
                {
                    result.Values = listConstraints.Values.Select(v => new ListValue
                    {
                        Value = v.Value,
                        Modifier = v.Modifier
                    });
                }

                return result;
            }
            
            private static AttributeSpecificationType ResolveType(
                DbSpecification source)
            {
                switch (source.Constraints)
                {
                    case DbNumberConstraints _:
                        return AttributeSpecificationType.Number;
                    case DbStringConstraints _:
                        return AttributeSpecificationType.String;
                    case DbListConstraints _:
                        return AttributeSpecificationType.List;
                    default:
                        return AttributeSpecificationType.Number;
                }
            }
        }
    }
}
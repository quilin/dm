using AutoMapper;
using DbSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;
using DbSpecification = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSpecification;
using DbListValue = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.ListAttributeValue;

namespace DM.Services.Gaming.Dto.Shared;

/// <inheritdoc />
internal class AttributeSchemaProfile : Profile
{
    /// <inheritdoc />
    public AttributeSchemaProfile()
    {
        CreateMap<DbSchema, AttributeSchema>();
        CreateMap<DbSpecification, AttributeSpecification>()
            .ConvertUsing<SpecificationConverter>();
        CreateMap<DbListValue, ListValue>();
    }
}
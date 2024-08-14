using AutoMapper;

namespace DM.Web.API.Dto.Games.Attributes;

/// <inheritdoc />
internal class AttributeSchemaProfile : Profile
{
    /// <inheritdoc />
    public AttributeSchemaProfile()
    {
        CreateMap<DM.Services.Gaming.Dto.Shared.AttributeSchema, AttributeSchema>()
            .ReverseMap();

        CreateMap<DM.Services.Gaming.Dto.Shared.AttributeSpecification, AttributeSpecification>()
            .ReverseMap();

        CreateMap<DM.Services.Gaming.Dto.Shared.ListValue, AttributeValueSpecification>()
            .ReverseMap();
    }
}
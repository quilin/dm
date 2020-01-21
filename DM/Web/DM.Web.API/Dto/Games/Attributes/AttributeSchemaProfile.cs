using AutoMapper;
using DM.Services.Core.Extensions;

namespace DM.Web.API.Dto.Games.Attributes
{
    /// <inheritdoc />
    public class AttributeSchemaProfile : Profile
    {
        /// <inheritdoc />
        public AttributeSchemaProfile()
        {
            CreateMap<DM.Services.Gaming.Dto.Shared.AttributeSchema, AttributeSchema>()
                .ForMember(s => s.Id, d => d.MapFrom(s => s.Id.EncodeToReadable(s.Title)));

            CreateMap<DM.Services.Gaming.Dto.Shared.AttributeSpecification, AttributeSpecification>()
                .ForMember(s => s.Id, d => d.MapFrom(s => s.Id.EncodeToReadable(s.Title)));
        }
    }
}
using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using DM.Services.Search.Dto;
using DM.Services.Search.Grpc;

namespace DM.Services.Search.Consumer.Implementation;

internal class SearchProfile : Profile
{
    public SearchProfile()
    {
        CreateMap<SearchEntityType, Core.Dto.Enums.SearchEntityType>()
            .ConvertUsingEnumMapping()
            .ReverseMap();

        CreateMap<FoundEntity, SearchResponse.Types.SearchResultEntity>()
            .ForMember(d => d.Id, s => s.MapFrom(e => e.Id.ToString()));
    }
}
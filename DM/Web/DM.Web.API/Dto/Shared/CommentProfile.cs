using AutoMapper;
using DM.Services.Common.Dto;

namespace DM.Web.API.Dto.Shared;

/// <summary>
/// Mapping profile from Service DTO to API DTO for commentaries
/// </summary>
internal class CommentProfile : Profile
{
    /// <inheritdoc />
    public CommentProfile()
    {
        CreateMap<DM.Services.Common.Dto.Comment, Comment>()
            .ForMember(d => d.Created, s => s.MapFrom(c => c.CreateDate))
            .ForMember(d => d.Updated, s => s.MapFrom(c => c.LastUpdateDate));

        CreateMap<Comment, CreateComment>();
        CreateMap<Comment, UpdateComment>();
    }
}
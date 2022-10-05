using System.Linq;
using AutoMapper;

namespace DM.Services.Forum.Dto;

/// <summary>
/// Profile for forum DTO and DAL mapping
/// </summary>
internal class ForumProfile : Profile
{
    /// <inheritdoc />
    public ForumProfile()
    {
        CreateMap<Output.Forum, Output.Forum>();

        CreateMap<DataAccess.BusinessObjects.Fora.Forum, Output.Forum>()
            .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId))
            .ForMember(d => d.ModeratorIds,
                s => s.MapFrom(f => f.Moderators.Select(m => m.UserId)));
    }
}
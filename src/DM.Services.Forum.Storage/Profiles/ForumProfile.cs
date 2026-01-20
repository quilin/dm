using AutoMapper;

namespace DM.Services.Forum.Storage.Profiles;

/// <summary>
/// Profile for forum DTO and DAL mapping
/// </summary>
internal class ForumProfile : Profile
{
    /// <inheritdoc />
    public ForumProfile()
    {
        CreateMap<DataAccess.BusinessObjects.Fora.Forum, Dto.Output.Forum>()
            .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId))
            .ForMember(d => d.ModeratorIds,
                s => s.MapFrom(f => f.Moderators.Select(m => m.UserId)));
    }
}
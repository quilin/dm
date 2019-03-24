using System.Linq;
using AutoMapper;

namespace DM.Services.Forum.Dto
{
    /// <summary>
    /// Profile for forum DTO and DAL mapping
    /// </summary>
    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<DataAccess.BusinessObjects.Fora.Forum, Forum>()
                .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId))
                .ForMember(d => d.ModeratorIds,
                    s => s.MapFrom(f => f.Moderators.Select(m => m.UserId)));
        }
    }
}
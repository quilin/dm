using AutoMapper;
using DM.Services.Forum.Dto;

namespace DM.Web.API.Dto.Fora
{
    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<ForaListItem, Forum>()
                .ForMember(d => d.Id, s => s.MapFrom(f => f.Title));
        }
    }
}
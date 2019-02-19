using AutoMapper;

namespace DM.Web.API.Dto.Fora
{
    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<DM.Services.Forum.Dto.Forum, Forum>()
                .ForMember(d => d.Id, s => s.MapFrom(f => f.Title));
        }
    }
}
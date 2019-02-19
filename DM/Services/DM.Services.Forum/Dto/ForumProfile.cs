using AutoMapper;

namespace DM.Services.Forum.Dto
{
    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<DataAccess.BusinessObjects.Fora.Forum, Forum>()
                .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId));
        }
    }
}
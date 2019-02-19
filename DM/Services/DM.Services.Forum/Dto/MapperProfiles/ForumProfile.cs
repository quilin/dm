using AutoMapper;

namespace DM.Services.Forum.Dto.MapperProfiles
{
    public class ForumProfile : Profile
    {
        public ForumProfile()
        {
            CreateMap<DataAccess.BusinessObjects.Fora.Forum, ForaListItem>()
                .ForMember(d => d.Id, s => s.MapFrom(f => f.ForumId));
        }
    }
}
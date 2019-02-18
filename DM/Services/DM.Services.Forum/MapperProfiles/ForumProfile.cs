using AutoMapper;
using DM.Services.Forum.Dto;

namespace DM.Services.Forum.MapperProfiles
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
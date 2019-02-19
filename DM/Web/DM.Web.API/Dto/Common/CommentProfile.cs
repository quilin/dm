using AutoMapper;

namespace DM.Web.API.Dto.Common
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<DM.Services.Common.Dto.Comment, Comment>()
                .ForMember(d => d.Id, s => s.MapFrom(c => c.Id.ToString()));
        }
    }
}
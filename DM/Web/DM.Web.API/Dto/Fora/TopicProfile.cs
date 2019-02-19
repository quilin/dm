using AutoMapper;
using DM.Services.Forum.Dto;
using DM.Web.Core.Helpers;

namespace DM.Web.API.Dto.Fora
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<TopicsListItem, Topic>()
                .ForMember(d => d.Id, s => s.MapFrom(t => t.Id.EncodeToReadable(t.Title)))
                .ForMember(d => d.Created, s => s.MapFrom(t => t.CreateDate))
                .ForMember(d => d.CommentsCount, s => s.MapFrom(t => t.TotalCommentsCount))
                .ForMember(d => d.Description, s => s.MapFrom(t => t.Text));
            CreateMap<LastComment, LastTopicComment>()
                .ForMember(d => d.Created, s => s.MapFrom(c => c.CreateDate));
        }
    }
}
using AutoMapper;
using DM.Services.Forum.Dto;
using DM.Web.Core.Helpers;

namespace DM.Web.API.Dto.Fora
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<DM.Services.Forum.Dto.Topic, Topic>()
                .ForMember(d => d.Id, s => s.MapFrom(t => t.Id.EncodeToReadable(t.Title)))
                .ForMember(d => d.Created, s => s.MapFrom(t => t.CreateDate))
                .ForMember(d => d.CommentsCount, s => s.MapFrom(t => t.TotalCommentsCount))
                .ForMember(d => d.Description, s => s.MapFrom(t => t.Text))
                .ReverseMap()
                .ForMember(d => d.Id, s => s.Ignore())
                .ForMember(d => d.Forum, s => s.Ignore())
                .ForMember(d => d.Text, s => s.MapFrom(t => t.Description))
                .ForMember(d => d.CreateDate, s => s.Ignore())
                .ForMember(d => d.Author, s => s.Ignore())
                .ForMember(d => d.Attached, s => s.Ignore())
                .ForMember(d => d.Closed, s => s.Ignore())
                .ForMember(d => d.LastComment, s => s.Ignore())
                .ForMember(d => d.LastActivityDate, s => s.Ignore());
            CreateMap<LastComment, LastTopicComment>()
                .ForMember(d => d.Created, s => s.MapFrom(c => c.CreateDate))
                .ReverseMap();
        }
    }
}
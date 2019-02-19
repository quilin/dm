using System.Linq;
using AutoMapper;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;

namespace DM.Services.Forum.Dto
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<LastComment, Comment>();
            CreateMap<ForumTopic, Topic>()
                .ForMember(d => d.Id, s => s.MapFrom(t => t.ForumTopicId))
                .ForMember(d => d.LastActivityDate, s => s.MapFrom(t => t.LastComment == null
                    ? t.CreateDate
                    : t.LastComment.CreateDate))
                .ForMember(d => d.TotalCommentsCount, s => s.MapFrom(t => t.Comments.Count(c => !c.IsRemoved)));

            CreateMap<Topic, ForumTopic>()
                .ForMember(d => d.Forum, s => s.Ignore())
                .ForMember(d => d.Author, s => s.Ignore())
                .ForMember(d => d.Likes, s => s.Ignore())
                .ForMember(d => d.LastComment, s => s.Ignore())
                .ForMember(d => d.Comments, s => s.Ignore())
                .ForMember(d => d.CreateDate, s => s.Ignore())
                .ForMember(d => d.Attached, s => s.Ignore())
                .ForMember(d => d.Closed, s => s.Ignore())
                .ForMember(d => d.IsRemoved, s => s.Ignore())
                .ForMember(d => d.ForumTopicId, s => s.MapFrom(t => t.Id))
                .ForMember(d => d.UserId, s => s.MapFrom(t => t.Author.UserId))
                .ForMember(d => d.ForumId, s => s.MapFrom(t => t.Forum.Id));
        }
    }
}
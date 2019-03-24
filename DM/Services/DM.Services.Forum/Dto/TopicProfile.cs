using System.Linq;
using AutoMapper;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.DataAccess.BusinessObjects.Fora;

namespace DM.Services.Forum.Dto
{
    /// <summary>
    /// Profile for topic DTO and DAL mapping
    /// </summary>
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
                .ForMember(d => d.TotalCommentsCount, s => s.MapFrom(t => t.Comments.Count(c => !c.IsRemoved)))
                .ForMember(d => d.Likes, s => s.MapFrom(t => t.Likes.Select(l => l.User)));
        }
    }
}
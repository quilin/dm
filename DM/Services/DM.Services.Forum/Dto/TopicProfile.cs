using System.Linq;
using AutoMapper;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.Dto
{
    /// <summary>
    /// Profile for topic DTO and DAL mapping
    /// </summary>
    public class TopicProfile : Profile
    {
        /// <inheritdoc />
        public TopicProfile()
        {
            CreateMap<LastComment, ForumComment>();
            CreateMap<ForumTopic, Topic>()
                .ForMember(d => d.Id, s => s.MapFrom(t => t.ForumTopicId))
                .ForMember(d => d.LastActivityDate, s => s.MapFrom(t => t.LastForumComment == null
                    ? t.CreateDate
                    : t.LastForumComment.CreateDate))
                .ForMember(d => d.LastComment, s => s.MapFrom(t => t.LastForumComment))
                .ForMember(d => d.TotalCommentsCount, s => s.MapFrom(t => t.Comments.Count(c => !c.IsRemoved)))
                .ForMember(d => d.Likes, s => s.MapFrom(t => t.Likes.Select(l => l.User)));
        }
    }
}
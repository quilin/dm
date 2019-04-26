using System.Linq;
using AutoMapper;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto.Internal;
using DM.Services.Forum.Dto.Output;

namespace DM.Services.Forum.Dto
{
    /// <summary>
    /// Profile for comment mapping
    /// </summary>
    public class CommentProfile : Profile
    {
        /// <inheritdoc />
        public CommentProfile()
        {
            CreateMap<ForumComment, Comment>()
                .ForMember(d => d.Id, s => s.MapFrom(c => c.ForumCommentId))
                .ForMember(d => d.Likes, s => s.MapFrom(c => c.Likes.Select(l => l.User)));

            CreateMap<ForumComment, CommentToDelete>()
                .ForMember(d => d.Id, s => s.MapFrom(c => c.ForumCommentId))
                .ForMember(d => d.TopicId, s => s.MapFrom(c => c.ForumTopicId))
                .ForMember(d => d.IsLastCommentOfTopic, s => s.MapFrom(c => c.ForumCommentId == c.Topic.LastCommentId));
        }
    }
}
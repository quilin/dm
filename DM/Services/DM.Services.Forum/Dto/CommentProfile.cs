using System.Linq;
using AutoMapper;
using DM.Services.Forum.Dto.Internal;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

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
            CreateMap<Comment, Output.Comment>()
                .ForMember(d => d.Id, s => s.MapFrom(c => c.CommentId))
                .ForMember(d => d.Likes, s => s.MapFrom(c => c.Likes.Select(l => l.User)));

            CreateMap<Comment, CommentToDelete>()
                .ForMember(d => d.Id, s => s.MapFrom(c => c.CommentId))
                .ForMember(d => d.TopicId, s => s.MapFrom(c => c.EntityId))
                .ForMember(d => d.IsLastCommentOfTopic, s => s.MapFrom(c => c.CommentId == c.Topic.LastCommentId));
        }
    }
}
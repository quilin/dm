using System.Linq;
using AutoMapper;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto.Output;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.Dto;

/// <summary>
/// Profile for topic DTO and DAL mapping
/// </summary>
internal class TopicProfile : Profile
{
    /// <inheritdoc />
    public TopicProfile()
    {
        CreateMap<Comment, LastComment>();

        CreateMap<ForumTopic, Topic>()
            .ForMember(d => d.Id, s => s.MapFrom(t => t.ForumTopicId))
            .ForMember(d => d.LastActivityDate, s => s.MapFrom(t => t.LastComment == null
                ? t.CreateDate
                : t.LastComment.CreateDate))
            .ForMember(d => d.TotalCommentsCount, s => s.MapFrom(t => t.Comments.Count(c => !c.IsRemoved)))
            .ForMember(d => d.Likes, s => s.MapFrom(t => t.Likes.Select(l => l.User)));
    }
}
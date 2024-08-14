using AutoMapper;
using Comment = DM.Services.DataAccess.BusinessObjects.Common.Comment;

namespace DM.Services.Forum.Dto.Internal;

/// <summary>
/// Profile for comment mapping
/// </summary>
internal class CommentProfile : Profile
{
    /// <inheritdoc />
    public CommentProfile()
    {
        CreateMap<Comment, CommentToDelete>()
            .ForMember(d => d.Id, s => s.MapFrom(c => c.CommentId))
            .ForMember(d => d.Likes, s => s.Ignore())
            .ForMember(d => d.IsLastCommentOfTopic, s => s.MapFrom(c => c.CommentId == c.Topic.LastCommentId));
    }
}
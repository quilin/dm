using System.Linq;
using AutoMapper;
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
            CreateMap<DataAccess.BusinessObjects.Fora.ForumComment, Comment>()
                .ForMember(d => d.Id, s => s.MapFrom(c => c.ForumCommentId))
                .ForMember(d => d.Likes, s => s.MapFrom(c => c.Likes.Select(l => l.User)));
        }
    }
}
using System.Linq;
using AutoMapper;

namespace DM.Services.Common.Dto;

/// <summary>
/// Profile for comment mapping
/// </summary>
internal class CommentProfile : Profile
{
    /// <inheritdoc />
    public CommentProfile()
    {
        CreateMap<DataAccess.BusinessObjects.Common.Comment, Comment>()
            .ForMember(d => d.Id, s => s.MapFrom(c => c.CommentId))
            .ForMember(d => d.Likes, s => s.MapFrom(c => c.Likes.Select(l => l.User)));
    }
}
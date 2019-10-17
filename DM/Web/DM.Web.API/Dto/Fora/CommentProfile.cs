using AutoMapper;
using DM.Services.Forum.Dto.Input;
using DM.Web.Core.Helpers;

namespace DM.Web.API.Dto.Fora
{
    /// <summary>
    /// Mapping profile from Service DTO to API DTO for commentaries
    /// </summary>
    public class CommentProfile : Profile
    {
        /// <inheritdoc />
        public CommentProfile()
        {
            CreateMap<DM.Services.Common.Dto.Comment, Comment>()
                .ForMember(d => d.Id, s => s.MapFrom(c => c.Id.EncodeToReadable("")))
                .ForMember(d => d.Created, s => s.MapFrom(c => c.CreateDate))
                .ForMember(d => d.Updated, s => s.MapFrom(c => c.LastUpdateDate));

            CreateMap<Comment, CreateComment>();

            CreateMap<Comment, UpdateComment>();
        }
    }
}
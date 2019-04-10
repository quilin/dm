using AutoMapper;
using DM.Services.Forum.Dto.Input;

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
            CreateMap<DM.Services.Forum.Dto.Output.Comment, Comment>()
                .ForMember(d => d.Id, s => s.MapFrom(c => c.Id.ToString()));

            CreateMap<Comment, CreateComment>();

            CreateMap<Comment, UpdateComment>();
        }
    }
}
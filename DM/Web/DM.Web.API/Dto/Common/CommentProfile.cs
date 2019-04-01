using AutoMapper;

namespace DM.Web.API.Dto.Common
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
                .ForMember(d => d.Id, s => s.MapFrom(c => c.Id.ToString()));
        }
    }
}
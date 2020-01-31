using AutoMapper;

namespace DM.Web.API.Dto.Community
{
    /// <inheritdoc />
    public class ReviewProfile : Profile
    {
        /// <inheritdoc />
        public ReviewProfile()
        {
            CreateMap<DM.Services.Community.BusinessProcesses.Reviews.Reading.Review, Review>();
        }
    }
}
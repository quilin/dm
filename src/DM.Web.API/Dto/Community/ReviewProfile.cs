using AutoMapper;
using DM.Services.Community.BusinessProcesses.Reviews.Creating;
using DM.Services.Community.BusinessProcesses.Reviews.Updating;

namespace DM.Web.API.Dto.Community;

/// <inheritdoc />
internal class ReviewProfile : Profile
{
    /// <inheritdoc />
    public ReviewProfile()
    {
        CreateMap<DM.Services.Community.BusinessProcesses.Reviews.Reading.Review, Review>();
        CreateMap<Review, CreateReview>();
        CreateMap<Review, UpdateReview>()
            .ForMember(d => d.ReviewId, s => s.MapFrom(r => r.Id));
    }
}
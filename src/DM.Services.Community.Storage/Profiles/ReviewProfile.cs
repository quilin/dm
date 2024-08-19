using AutoMapper;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DbReview = DM.Services.DataAccess.BusinessObjects.Common.Review;

namespace DM.Services.Community.Storage.Profiles;

/// <inheritdoc />
internal class ReviewProfile : Profile
{
    /// <inheritdoc />
    public ReviewProfile()
    {
        CreateMap<DbReview, Review>()
            .ForMember(d => d.Id, s => s.MapFrom(r => r.ReviewId))
            .ForMember(d => d.Approved, s => s.MapFrom(r => r.IsApproved));
    }
}
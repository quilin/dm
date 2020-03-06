using DM.Services.Community.BusinessProcesses.Reviews.Reading;

namespace DM.Web.Classic.Views.Home.Reviews
{
    public interface IReviewViewModelBuilder
    {
        ReviewViewModel Build(Review review, bool isRandom = false);
    }
}
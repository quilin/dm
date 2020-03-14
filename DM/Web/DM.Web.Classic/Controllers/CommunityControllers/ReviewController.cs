using System;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Reviews.Creating;
using DM.Services.Community.BusinessProcesses.Reviews.Deleting;
using DM.Services.Community.BusinessProcesses.Reviews.Updating;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.Home.CreateReview;
using DM.Web.Classic.Views.Home.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommunityControllers
{
    public class ReviewController : DmControllerBase
    {
        private readonly IReviewCreatingService creatingService;
        private readonly IReviewUpdatingService updatingService;
        private readonly IReviewDeletingService deletingService;
        private readonly IReviewViewModelBuilder viewModelBuilder;

        public ReviewController(
            IReviewCreatingService creatingService,
            IReviewUpdatingService updatingService,
            IReviewDeletingService deletingService,
            IReviewViewModelBuilder viewModelBuilder)
        {
            this.creatingService = creatingService;
            this.updatingService = updatingService;
            this.deletingService = deletingService;
            this.viewModelBuilder = viewModelBuilder;
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> Create(CreateReviewForm form)
        {
            return Ok(await creatingService.Create(new CreateReview
            {
                Text = form.Text
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Guid reviewId)
        {
            var review = await updatingService.Update(new UpdateReview
            {
                ReviewId = reviewId,
                Approved = true
            });
            var reviewViewModel = viewModelBuilder.Build(review);
            return PartialView("~/Views/Home/Reviews/Review.cshtml", reviewViewModel);
        }

        [HttpPost]
        public Task Remove(Guid reviewId) => deletingService.Delete(reviewId);
    }
}
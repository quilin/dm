using System;
using DM.Web.Classic.Views.Home.CreateReview;
using DM.Web.Classic.Views.Home.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ReviewControllers
{
    public class ReviewController : DmControllerBase
    {
        private readonly IReviewService reviewService;
        private readonly IReviewViewModelBuilder reviewViewModelBuilder;

        public ReviewController(
            IReviewService reviewService,
            IReviewViewModelBuilder reviewViewModelBuilder
            )
        {
            this.reviewService = reviewService;
            this.reviewViewModelBuilder = reviewViewModelBuilder;
        }

        [HttpPost, ValidationRequired]
        public void Create(CreateReviewForm form)
        {
            reviewService.Create(form.Text);
        }

        [HttpPost]
        public ActionResult Approve(Guid reviewId)
        {
            var review = reviewService.Approve(reviewId);
            var reviewViewModel = reviewViewModelBuilder.Build(review);
            return PartialView("~/Views/Home/Reviews/Review.cshtml", reviewViewModel);
        }

        [HttpPost]
        public void Remove(Guid reviewId)
        {
            reviewService.Remove(reviewId);
        }
    }
}
using System;
using System.Net;
using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Views.Rating;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.RatingControllers
{
    public class RatingController : DmControllerBase
    {
        private readonly IVoteFormBuilder voteFormBuilder;
        private readonly IRatingService ratingService;
        private readonly IPostService postService;
        private readonly IVoteResolver voteResolver;
        private readonly IUserProvider userProvider;

        public RatingController(
            IVoteFormBuilder voteFormBuilder,
            IRatingService ratingService,
            IPostService postService,
            IVoteResolver voteResolver,
            IUserProvider userProvider
            )
        {
            this.voteFormBuilder = voteFormBuilder;
            this.ratingService = ratingService;
            this.postService = postService;
            this.voteResolver = voteResolver;
            this.userProvider = userProvider;
        }

        [HttpGet]
        public IActionResult Vote(Guid postId, VoteSign voteSign)
        {
            var post = postService.Read(postId);
            if (!voteResolver.TryVote(userProvider.Current, post, voteSign, out var errorMessage))
            {
                return AjaxError(HttpStatusCode.Forbidden, errorMessage);
            }

            var voteForm = voteFormBuilder.Build(postId, voteSign);
            return View(voteForm);
        }

        [HttpPost]
        public void Vote(VoteForm voteForm)
        {
            ratingService.Create(voteForm.PostId, voteForm.Sign, VoteType.Roleplay, voteForm.Comment);
        }
    }
}
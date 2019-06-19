using System;
using DM.Services.Common.Authorization;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Services.Forum.BusinessProcesses.Commentaries.Updating;
using DM.Services.Forum.Dto.Input;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.Shared.Commentaries;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommentariesControllers
{
    public class EditCommentaryController : DmControllerBase
    {
        private readonly ICommentaryReadingService commentaryReadingService;
        private readonly IEditCommentaryFormBuilder editCommentaryFormBuilder;
        private readonly ICommentaryUpdatingService commentaryUpdatingService;
        private readonly IIntentionManager intentionsManager;
        private readonly ICommentaryViewModelBuilder commentaryViewModelBuilder;

        public EditCommentaryController(
            ICommentaryReadingService commentaryReadingService,
            IEditCommentaryFormBuilder editCommentaryFormBuilder,
            ICommentaryUpdatingService commentaryUpdatingService,
            IIntentionManager intentionsManager,
            ICommentaryViewModelBuilder commentaryViewModelBuilder
            )
        {
            this.commentaryReadingService = commentaryReadingService;
            this.editCommentaryFormBuilder = editCommentaryFormBuilder;
            this.commentaryUpdatingService = commentaryUpdatingService;
            this.intentionsManager = intentionsManager;
            this.commentaryViewModelBuilder = commentaryViewModelBuilder;
        }

        [HttpGet]
        public ActionResult Edit(Guid commentaryId)
        {
            var comment = commentaryReadingService.Get(commentaryId).Result;
            intentionsManager.ThrowIfForbidden(CommentIntention.Edit, comment);
            var editCommentaryForm = editCommentaryFormBuilder.Build(comment);
            return PartialView("Commentaries/EditCommentary", editCommentaryForm);
        }

        [HttpPost, ValidationRequired]
        public ActionResult Edit(EditCommentaryForm editCommentaryForm)
        {
            var updateComment = new UpdateComment
            {
                CommentId = editCommentaryForm.CommentaryId,
                Text = editCommentaryForm.Text
            };
            var comment = commentaryUpdatingService.Update(updateComment).Result;
            var commentaryViewModel = commentaryViewModelBuilder.Build(comment);
            return PartialView("Commentaries/Commentary", commentaryViewModel);
        }
    }
}
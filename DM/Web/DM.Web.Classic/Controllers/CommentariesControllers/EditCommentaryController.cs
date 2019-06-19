using System;
using DM.Web.Classic.Views.Shared.Commentaries;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommentariesControllers
{
    public class EditCommentaryController : DmControllerBase
    {
        private readonly IEditCommentaryFormBuilder editCommentaryFormBuilder;
        private readonly ICommentService commentService;
        private readonly ILikeService likeService;
        private readonly IIntentionsManager intentionsManager;
        private readonly ICommentaryViewModelBuilder commentaryViewModelBuilder;

        public EditCommentaryController(
            IEditCommentaryFormBuilder editCommentaryFormBuilder,
            ICommentService commentService,
            ILikeService likeService,
            IIntentionsManager intentionsManager,
            ICommentaryViewModelBuilder commentaryViewModelBuilder
            )
        {
            this.editCommentaryFormBuilder = editCommentaryFormBuilder;
            this.commentService = commentService;
            this.likeService = likeService;
            this.intentionsManager = intentionsManager;
            this.commentaryViewModelBuilder = commentaryViewModelBuilder;
        }

        [HttpGet]
        public ActionResult Edit(Guid commentaryId)
        {
            var comment = commentService.Read(commentaryId);
            intentionsManager.ThrowIfForbidden(CommentIntention.Edit, comment);
            var editCommentaryForm = editCommentaryFormBuilder.Build(comment);
            return PartialView("Commentaries/EditCommentary", editCommentaryForm);
        }

        [HttpPost, ValidationRequired]
        public ActionResult Edit(EditCommentaryForm editCommentaryForm)
        {
            var comment = commentService.Update(editCommentaryForm.CommentaryId, editCommentaryForm.Text);
            var likes = likeService.Select(new[] {comment.CommentId});
            var commentaryViewModel = commentaryViewModelBuilder.Build(comment, likes);
            return PartialView("Commentaries/Commentary", commentaryViewModel);
        }
    }
}
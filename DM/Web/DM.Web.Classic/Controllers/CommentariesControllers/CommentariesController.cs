using System;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Shared.Commentaries;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommentariesControllers
{
    public class CommentariesController : DmControllerBase
    {
        private readonly ICommentariesViewModelBuilder commentariesViewModelBuilder;
        private readonly IUserProvider userProvider;
        private readonly ICommentService commentService;
        private readonly ILikeService likeService;

        public CommentariesController(
            ICommentariesViewModelBuilder commentariesViewModelBuilder,
            IUserProvider userProvider,
            ICommentService commentService,
            ILikeService likeService
            )
        {
            this.commentariesViewModelBuilder = commentariesViewModelBuilder;
            this.userProvider = userProvider;
            this.commentService = commentService;
            this.likeService = likeService;
        }
        
        public ActionResult Index(Guid entityId, int entityNumber)
        {
            if (Request.IsAjaxRequest())
            {
                var commentaries = commentariesViewModelBuilder.BuildList(entityId, entityNumber);
                return PartialView("Commentaries/CommentariesList", commentaries);
            }
            var commentariesViewModel = commentariesViewModelBuilder.Build(entityId, entityNumber);
            return PartialView("Commentaries/Commentaries", commentariesViewModel);
        }

        [HttpPost]
        public int Remove(Guid commentaryId)
        {
            var comment = commentService.Remove(commentaryId);
            return PagingHelper.GetTotalPages(commentService.Count(comment.EntityId), userProvider.CurrentSettings.CommentsPerPage);
        }

        [HttpPost]
        public ActionResult ToggleLike(Guid commentaryId)
        {
            var created = likeService.ToggleCommentLike(commentaryId);
            return Json(new
            {
                likesCount = likeService.Select(new[] { commentaryId }).Length, created
            });
        }
    }
}
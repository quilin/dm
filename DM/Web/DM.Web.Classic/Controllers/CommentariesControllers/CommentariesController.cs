using System;
using DM.Services.Forum.BusinessProcesses.Commentaries.Deleting;
using DM.Services.Forum.BusinessProcesses.Likes;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Shared.Commentaries;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommentariesControllers
{
    public class CommentariesController : DmControllerBase
    {
        private readonly ICommentaryDeletingService commentaryDeletingService;
        private readonly ILikeService likeService;
        private readonly ICommentariesViewModelBuilder commentariesViewModelBuilder;

        public CommentariesController(
            ICommentaryDeletingService commentaryDeletingService,
            ILikeService likeService,
            ICommentariesViewModelBuilder commentariesViewModelBuilder)
        {
            this.commentaryDeletingService = commentaryDeletingService;
            this.likeService = likeService;
            this.commentariesViewModelBuilder = commentariesViewModelBuilder;
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
            commentaryDeletingService.Delete(commentaryId).Wait();
            return 1;
        }

        [HttpPost]
        public ActionResult ToggleLike(Guid commentaryId)
        {
            bool created;
            try
            {
                likeService.LikeComment(commentaryId).Wait();
                created = true;
            }
            catch
            {
                likeService.DislikeComment(commentaryId).Wait();
                created = false;
            }
            return Json(new
            {
                likesCount = 1, created
            });
        }
    }
}
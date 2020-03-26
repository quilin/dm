using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Forum.BusinessProcesses.Commentaries.Deleting;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Services.Forum.BusinessProcesses.Likes;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.Shared.Commentaries;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommentariesControllers
{
    public class CommentariesController : Controller
    {
        private readonly ICommentaryDeletingService commentaryDeletingService;
        private readonly ICommentaryReadingService commentaryReadingService;
        private readonly ILikeService likeService;
        private readonly IIdentityProvider identityProvider;
        private readonly ICommentariesViewModelBuilder commentariesViewModelBuilder;

        public CommentariesController(
            ICommentaryDeletingService commentaryDeletingService,
            ICommentaryReadingService commentaryReadingService,
            ILikeService likeService,
            IIdentityProvider identityProvider,
            ICommentariesViewModelBuilder commentariesViewModelBuilder)
        {
            this.commentaryDeletingService = commentaryDeletingService;
            this.commentaryReadingService = commentaryReadingService;
            this.likeService = likeService;
            this.identityProvider = identityProvider;
            this.commentariesViewModelBuilder = commentariesViewModelBuilder;
        }
        
        public async Task<IActionResult> Index(Guid entityId, int entityNumber)
        {
            if (Request.IsAjaxRequest())
            {
                var commentaries = await commentariesViewModelBuilder.BuildList(entityId, entityNumber);
                return PartialView("Commentaries/CommentariesList", commentaries);
            }
            var commentariesViewModel = await commentariesViewModelBuilder.Build(entityId, entityNumber, false);
            return PartialView("Commentaries/Commentaries", commentariesViewModel);
        }

        [HttpPost]
        public async Task<int> Remove(Guid commentaryId, Guid entityId)
        {
            await commentaryDeletingService.Delete(commentaryId);
            var (_, paging) = await commentaryReadingService.Get(entityId, PagingQuery.Empty);
            return paging.TotalPagesCount;
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLike(Guid commentaryId)
        {
            var comment = await commentaryReadingService.Get(commentaryId);
            var userLiked = comment.Likes.Any(l => l.UserId == identityProvider.Current.User.UserId);
            if (userLiked)
            {
                await likeService.DislikeComment(commentaryId);
            }
            else
            {
                await likeService.LikeComment(commentaryId);
            }
            return Json(new
            {
                likesCount = comment.Likes.Count() + (userLiked ? -1 : 1),
                created = !userLiked
            });
        }
    }
}
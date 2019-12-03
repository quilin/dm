using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Deleting;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;
using DM.Services.Gaming.BusinessProcesses.Likes;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.GameCommentaries;
using DM.Web.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.GameCommentariesControllers
{
    public class GameCommentariesController : DmControllerBase
    {
        private readonly ICommentaryReadingService commentaryReadingService;
        private readonly ICommentaryDeletingService commentaryDeletingService;
        private readonly IGameCommentariesViewModelBuilder gameCommentariesViewModelBuilder;
        private readonly ILikeService likeService;
        private readonly IIdentity identity;

        public GameCommentariesController(
            ICommentaryReadingService commentaryReadingService,
            ICommentaryDeletingService commentaryDeletingService,
            IGameCommentariesViewModelBuilder gameCommentariesViewModelBuilder,
            ILikeService likeService,
            IIdentityProvider identityProvider)
        {
            this.commentaryReadingService = commentaryReadingService;
            this.commentaryDeletingService = commentaryDeletingService;
            this.gameCommentariesViewModelBuilder = gameCommentariesViewModelBuilder;
            this.likeService = likeService;
            identity = identityProvider.Current;
        }

        public async Task<IActionResult> LastUnread(string gameIdEncoded)
        {
            gameIdEncoded.TryDecodeFromReadableGuid(out var gameId);
            var entityNumber = 1;

            var (_, paging, game) = await commentaryReadingService.Get(gameId, PagingQuery.Empty);
            if (paging.TotalEntitiesCount > 0)
            {
                entityNumber = Math.Min(paging.TotalEntitiesCount - game.UnreadCommentsCount + 1,
                    paging.TotalEntitiesCount);
            }

            return RedirectToAction("Index", new RouteValueDictionary
            {
                {"gameId", gameIdEncoded},
                {"entityNumber", entityNumber}
            });
        }

        public async Task<IActionResult> Index(Guid gameId, int entityNumber)
        {
            if (!Request.IsAjaxRequest())
            {
                var moduleCommentariesViewModel = await gameCommentariesViewModelBuilder.Build(gameId, entityNumber);
                return View("GameCommentaries", moduleCommentariesViewModel);
            }

            var commentaries = await gameCommentariesViewModelBuilder.BuildList(gameId, entityNumber);
            return View("GameCommentariesList", commentaries);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid commentaryId)
        {
            var comment = await commentaryReadingService.Get(commentaryId);
            await commentaryDeletingService.Delete(commentaryId);
            var (_, paging, _) = await commentaryReadingService.Get(comment.EntityId, PagingQuery.Empty);
            return Ok(paging.TotalPagesCount);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLike(Guid commentaryId)
        {
            var comment = await commentaryReadingService.Get(commentaryId);
            var created = comment.Likes.Any(l => l.UserId == identity.User.UserId);
            var likesCount = comment.Likes.Count();
            if (created)
            {
                await likeService.DislikeComment(commentaryId);
                return Json(new {likesCount = likesCount - 1, created = false});
            }

            await likeService.LikeComment(commentaryId);
            return Json(new {likesCount = likesCount + 1, created = true});
        }
    }
}
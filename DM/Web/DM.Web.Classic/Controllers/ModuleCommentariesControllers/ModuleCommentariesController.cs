using System;
using DM.Web.Classic.Extensions.RequestExtensions;
using DM.Web.Classic.Views.ModuleCommentaries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Controllers.ModuleCommentariesControllers
{
    public class ModuleCommentariesController : DmControllerBase
    {
        private readonly IModuleCommentariesViewModelBuilder moduleCommentariesViewModelBuilder;
        private readonly IModuleCommentService commentService;
        private readonly IModuleService moduleService;
        private readonly ILikeService likeService;
        private readonly IUnreadCounterService unreadCounterService;
        private readonly IUserProvider userProvider;

        public ModuleCommentariesController(
            IModuleCommentariesViewModelBuilder moduleCommentariesViewModelBuilder,
            IModuleCommentService commentService,
            IModuleService moduleService,
            ILikeService likeService,
            IUnreadCounterService unreadCounterService,
            IUserProvider userProvider)
        {
            this.moduleCommentariesViewModelBuilder = moduleCommentariesViewModelBuilder;
            this.commentService = commentService;
            this.moduleService = moduleService;
            this.likeService = likeService;
            this.unreadCounterService = unreadCounterService;
            this.userProvider = userProvider;
        }

        public ActionResult LastUnread(string moduleIdEncoded)
        {
            moduleIdEncoded.TryDecodeFromReadableGuid(out var moduleId);
            var entityNumber = 1;

            var totalCount = commentService.Count(moduleId);
            if (totalCount > 0)
            {
                var unreadCount = unreadCounterService.GetCounter(moduleId, EntryType.Message);
                entityNumber = Math.Min(totalCount - unreadCount + 1, totalCount);
            }

            return RedirectToAction("Index", new RouteValueDictionary
            {
                {"moduleId", moduleIdEncoded},
                {"entityNumber", entityNumber}
            });
        }

        public ActionResult Index(Guid moduleId, int entityNumber)
        {
            var module = moduleService.Read(moduleId);
            if (!Request.IsAjaxRequest())
            {
                var moduleCommentariesViewModel = moduleCommentariesViewModelBuilder.Build(module, entityNumber);
                return View("ModuleCommentaries", moduleCommentariesViewModel);
            }

            var commentaries = moduleCommentariesViewModelBuilder.BuildList(module, entityNumber);
            return View("ModuleCommentariesList", commentaries);
        }

        [HttpPost]
        public int Remove(Guid commentaryId)
        {
            var comment = commentService.Read(commentaryId);
            var module = moduleService.Read(comment.ModuleId);
            commentService.Remove(commentaryId);
            return PagingHelper.GetTotalPages(commentService.Count(module.ModuleId), userProvider.CurrentSettings.CommentsPerPage);
        }

        [HttpPost]
        public ActionResult ToggleLike(Guid commentaryId)
        {
            var created = likeService.ToggleModuleCommentLike(commentaryId);
            return Json(new
            {
                likesCount = likeService.Select(new[] { commentaryId }).Length,
                created
            });
        }
    }
}
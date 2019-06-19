using System;
using System.Collections.Generic;
using DM.Web.Classic.Views.ModuleCommentaries.Commentary;
using DM.Web.Classic.Views.ModuleCommentaries.EditCommentary;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ModuleCommentariesControllers
{
    public class EditModuleCommentaryController : DmControllerBase
    {
        private readonly IModuleCommentaryEditFormBuilder moduleCommentaryEditFormBuilder;
        private readonly IModuleService moduleService;
        private readonly IModuleCommentService commentService;
        private readonly ICharacterService characterService;
        private readonly ILikeService likeService;
        private readonly IIntentionsManager intentionsManager;
        private readonly IModuleCommentaryViewModelBuilder moduleCommentaryViewModelBuilder;

        public EditModuleCommentaryController(
            IModuleCommentaryEditFormBuilder moduleCommentaryEditFormBuilder,
            IModuleService moduleService,
            IModuleCommentService commentService,
            ICharacterService characterService,
            ILikeService likeService,
            IIntentionsManager intentionsManager,
            IModuleCommentaryViewModelBuilder moduleCommentaryViewModelBuilder
            )
        {
            this.moduleCommentaryEditFormBuilder = moduleCommentaryEditFormBuilder;
            this.moduleService = moduleService;
            this.commentService = commentService;
            this.characterService = characterService;
            this.likeService = likeService;
            this.intentionsManager = intentionsManager;
            this.moduleCommentaryViewModelBuilder = moduleCommentaryViewModelBuilder;
        }

        [HttpGet]
        public ActionResult Edit(Guid commentaryId)
        {
            var comment = commentService.Read(commentaryId);
            var module = moduleService.Read(comment.ModuleId);
            intentionsManager.ThrowIfForbidden(ModuleCommentIntention.Edit, comment, module);
            var moduleCommentaryEditForm = moduleCommentaryEditFormBuilder.Build(comment);
            return View("~/Views/ModuleCommentaries/EditCommentary/EditCommentary.cshtml", moduleCommentaryEditForm);
        }

        [HttpPost, ValidationRequired]
        public ActionResult Edit(ModuleCommentaryEditForm editForm)
        {
            var comment = commentService.Update(editForm.CommentaryId, editForm.Text);
            var module = moduleService.Read(comment.ModuleId);
            var characterNames = characterService.SelectByUserInModule(comment.UserId, comment.ModuleId, CharacterStatus.Active, CharacterStatus.Registration)
                .Select(c => c.Name)
                .ToArray();
            var likes = likeService.Select(new[] {comment.ModuleCommentId});
            var moduleCommentaryViewModel = moduleCommentaryViewModelBuilder.Build(comment, module, new Dictionary<Guid, string[]> {{comment.UserId, characterNames}}, likes);

            return View("~/Views/ModuleCommentaries/Commentary/Commentary.cshtml", moduleCommentaryViewModel);
        }
    }
}
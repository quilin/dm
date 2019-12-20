using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Gaming.BusinessProcesses.Characters.Reading;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Updating;
using DM.Services.Gaming.BusinessProcesses.Games.Reading;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.GameCommentaries.Commentary;
using DM.Web.Classic.Views.GameCommentaries.EditCommentary;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameCommentariesControllers
{
    public class EditGameCommentaryController : DmControllerBase
    {
        private readonly IGameCommentaryEditFormBuilder gameCommentaryEditFormBuilder;
        private readonly ICommentaryUpdatingService commentaryUpdatingService;
        private readonly IGameReadingService gameReadingService;
        private readonly ICharacterReadingService characterReadingService;
        private readonly IGameCommentaryViewModelBuilder commentaryViewModelBuilder;

        public EditGameCommentaryController(
            IGameCommentaryEditFormBuilder gameCommentaryEditFormBuilder,
            ICommentaryUpdatingService commentaryUpdatingService,
            IGameReadingService gameReadingService,
            ICharacterReadingService characterReadingService,
            IGameCommentaryViewModelBuilder commentaryViewModelBuilder)
        {
            this.gameCommentaryEditFormBuilder = gameCommentaryEditFormBuilder;
            this.commentaryUpdatingService = commentaryUpdatingService;
            this.gameReadingService = gameReadingService;
            this.characterReadingService = characterReadingService;
            this.commentaryViewModelBuilder = commentaryViewModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid commentaryId)
        {
            var form = await gameCommentaryEditFormBuilder.Build(commentaryId);
            return View("~/Views/GameCommentaries/EditCommentary/EditCommentary.cshtml", form);
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> Edit(GameCommentaryEditForm editForm)
        {
            var comment = await commentaryUpdatingService.Update(new UpdateComment
            {
                CommentId = editForm.CommentaryId,
                Text = editForm.Text
            });
            var game = await gameReadingService.GetGame(comment.EntityId);
            var characters = await characterReadingService.GetCharacters(game.Id);
            var characterNames = characters
                .ToLookup(c => c.Author.UserId)
                .ToDictionary(g => g.Key, g => g.Select(c => c.Name));
            var viewModel = commentaryViewModelBuilder.Build(comment, game, characterNames);

            return View("~/Views/GameCommentaries/Commentary/Commentary.cshtml", viewModel);
        }
    }
}
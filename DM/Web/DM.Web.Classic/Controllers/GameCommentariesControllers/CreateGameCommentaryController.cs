using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Creating;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.GameCommentaries.CreateCommentary;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.GameCommentariesControllers
{
    public class CreateGameCommentaryController : DmControllerBase
    {
        private readonly ICommentaryReadingService commentReadingService;
        private readonly ICommentaryCreatingService commentCreatingService;

        public CreateGameCommentaryController(
            ICommentaryReadingService commentReadingService,
            ICommentaryCreatingService commentCreatingService)
        {
            this.commentReadingService = commentReadingService;
            this.commentCreatingService = commentCreatingService;
        }

        [HttpPost, ValidationRequired]
        public async Task<IActionResult> Create(GameCommentaryCreateForm createForm)
        {
            await commentCreatingService.Create(new CreateComment
            {
                EntityId = createForm.GameId,
                Text = createForm.Text
            });
            await commentReadingService.MarkAsRead(createForm.GameId);
            var (_, paging, _) = await commentReadingService.Get(createForm.GameId, PagingQuery.Empty);
            return Ok(paging.TotalPagesCount);
        }
    }
}
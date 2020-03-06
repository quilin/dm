using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Core.Dto;
using DM.Services.Forum.BusinessProcesses.Commentaries.Creating;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.Shared.Commentaries;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommentariesControllers
{
    public class CreateCommentaryController : DmControllerBase
    {
        private readonly ICommentaryCreatingService commentaryCreatingService;
        private readonly ICommentaryReadingService commentaryReadingService;

        public CreateCommentaryController(
            ICommentaryCreatingService commentaryCreatingService,
            ICommentaryReadingService commentaryReadingService)
        {
            this.commentaryCreatingService = commentaryCreatingService;
            this.commentaryReadingService = commentaryReadingService;
        }

        [HttpPost, ValidationRequired]
        public async Task<int> Create(CreateCommentaryForm createCommentaryForm)
        {
            var createComment = new CreateComment
            {
                EntityId = createCommentaryForm.EntityId,
                Text = createCommentaryForm.Text
            };
            await commentaryCreatingService.Create(createComment);
            await commentaryReadingService.MarkAsRead(createComment.EntityId);
            var (_, paging) = await commentaryReadingService.Get(createCommentaryForm.EntityId, PagingQuery.Empty);
            return paging.TotalPagesCount;
        }
    }
}
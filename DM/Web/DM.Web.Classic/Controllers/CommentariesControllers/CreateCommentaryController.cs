using DM.Services.Forum.BusinessProcesses.Commentaries.Creating;
using DM.Services.Forum.Dto.Input;
using DM.Web.Classic.Middleware;
using DM.Web.Classic.Views.Shared.Commentaries;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommentariesControllers
{
    public class CreateCommentaryController : DmControllerBase
    {
        private readonly ICommentaryCreatingService commentaryCreatingService;

        public CreateCommentaryController(
            ICommentaryCreatingService commentaryCreatingService)
        {
            this.commentaryCreatingService = commentaryCreatingService;
        }

        [HttpPost, ValidationRequired]
        public int Create(CreateCommentaryForm createCommentaryForm)
        {
            var createComment = new CreateComment
            {
                TopicId = createCommentaryForm.EntityId,
                Text = createCommentaryForm.Text
            };
            commentaryCreatingService.Create(createComment).Wait();
            return 1;
        }
    }
}
using DM.Web.Classic.Views.Shared.Commentaries;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CommentariesControllers
{
    public class CreateCommentaryController : DmControllerBase
    {
        private readonly ICommentService commentService;
        private readonly IUserProvider userProvider;

        public CreateCommentaryController(
            ICommentService commentService,
            IUserProvider userProvider
        )
        {
            this.commentService = commentService;
            this.userProvider = userProvider;
        }

        [HttpPost, ValidationRequired]
        public int Create(CreateCommentaryForm createCommentaryForm)
        {
            commentService.Create(createCommentaryForm.EntityId, createCommentaryForm.Text);
            return PagingHelper.GetTotalPages(commentService.Count(createCommentaryForm.EntityId),
                userProvider.CurrentSettings.CommentsPerPage);
        }
    }
}
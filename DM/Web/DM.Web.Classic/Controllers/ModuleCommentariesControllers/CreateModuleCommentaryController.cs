using DM.Web.Classic.Views.ModuleCommentaries.CreateCommentary;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ModuleCommentariesControllers
{
    public class CreateModuleCommentaryController : DmControllerBase
    {
        private readonly IModuleCommentService commentService;
        private readonly IUserProvider userProvider;

        public CreateModuleCommentaryController(
            IModuleCommentService commentService,
            IUserProvider userProvider)
        {
            this.commentService = commentService;
            this.userProvider = userProvider;
        }

        [HttpPost, ValidationRequired]
        public int Create(ModuleCommentaryCreateForm createForm)
        {
            commentService.Create(createForm.ModuleId, createForm.Text);
            return PagingHelper.GetTotalPages(commentService.Count(createForm.ModuleId), userProvider.CurrentSettings.CommentsPerPage);
        }
    }
}
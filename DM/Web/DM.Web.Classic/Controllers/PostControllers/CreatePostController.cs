using DM.Web.Classic.Dto;
using DM.Web.Classic.Views.Room.CreatePost;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.PostControllers
{
    public class CreatePostController : DmControllerBase
    {
        private readonly IPostService postService;
        private readonly IUserProvider userProvider;
        private readonly IPostPreviewViewModelBuilder postPreviewViewModelBuilder;
        private readonly ICreatePostModelConverter createPostModelConverter;

        public CreatePostController(
            IPostService postService,
            IUserProvider userProvider,
            IPostPreviewViewModelBuilder postPreviewViewModelBuilder,
            ICreatePostModelConverter createPostModelConverter
            )
        {
            this.postService = postService;
            this.userProvider = userProvider;
            this.postPreviewViewModelBuilder = postPreviewViewModelBuilder;
            this.createPostModelConverter = createPostModelConverter;
        }

        [HttpPost, ValidationRequired]
        public int Create(CreatePostForm createForm)
        {
            var createPostModel = createPostModelConverter.Convert(createForm);
            postService.Create(createPostModel);
            return PagingHelper.GetTotalPages(postService.Count(createForm.RoomId), userProvider.CurrentSettings.PostsPerPage);
        }

        [HttpPost]
        public ActionResult Preview(CreatePostForm createForm)
        {
            var postViewModel = postPreviewViewModelBuilder.Build(createForm);
            return PartialView("~/Views/Room/CreatePost/Preview.cshtml", postViewModel);
        }
    }
}
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.ProfileControllers
{
    public class ProfilePictureUploadController : UploadControllerBase
    {
        private readonly IIntentionsManager intentionsManager;
        private readonly IUserService userService;
        private readonly IUserProvider userProvider;

        public ProfilePictureUploadController(
            IFileUploader fileUploader,
            IProgressHandler progressHandler,
            IIntentionsManager intentionsManager,
            IUserService userService,
            IUploadValidationOptionFactory validationOptionFactory,
            IUploadFileFactory uploadFileFactory,
            IUserProvider userProvider)
            : base(fileUploader, progressHandler, validationOptionFactory, uploadFileFactory, userProvider)
        {
            this.intentionsManager = intentionsManager;
            this.userService = userService;
            this.userProvider = userProvider;
        }

        public ActionResult GetForm(Guid userId)
        {
            return PartialView("~/Views/Profile/Upload/UploadForm.cshtml", userId);
        }

        [HttpPost]
        public void Upload(IFormFileCollection files, Guid uploadRootId, Guid userId)
        {
            var user = userService.Read(userId);
            intentionsManager.ThrowIfForbidden(UserIntention.Edit, user);

            Upload(uploadRootId, userId, UploadType.Public, files);
        }

        protected override void AfterUpload(Guid uploadId, Guid uploadRootId, Guid? userId)
        {
            if (userId.HasValue)
            {
                userService.UpdateProfilePicture(userId.Value, uploadId);
            }
        }

        [HttpPost]
        public void RemovePicture()
        {
            userService.UpdateProfilePicture(userProvider.Current.UserId, null);
        }

        protected override int MaxFilesCount => 1;

        protected override UploadValidationContext ValidationContext => base.ValidationContext
            .Add(ValidationOptionFactory.CreateMaxSizeOption(15 * 1 << 20, "Размер файла должен быть меньше 5Мб!"))
            .Add(ValidationOptionFactory.CreateAllowedTypesOption(UploadFileType.Image, "Файл должен быть изображением!"));
    }
}
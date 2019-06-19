using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public abstract class UploadControllerBase : DmControllerBase
    {
        private readonly IFileUploader fileUploader;
        private readonly IProgressHandler progressHandler;
        protected readonly IUploadValidationOptionFactory ValidationOptionFactory;
        private readonly IUploadFileFactory uploadFileFactory;
        private readonly IUserProvider userProvider;

        protected UploadControllerBase(
            IFileUploader fileUploader,
            IProgressHandler progressHandler,
            IUploadValidationOptionFactory validationOptionFactory,
            IUploadFileFactory uploadFileFactory,
            IUserProvider userProvider
            )
        {
            this.fileUploader = fileUploader;
            this.progressHandler = progressHandler;
            ValidationOptionFactory = validationOptionFactory;
            this.uploadFileFactory = uploadFileFactory;
            this.userProvider = userProvider;
        }

        public JsonResult Progress(Guid uploadId)
        {
            return Json(progressHandler.Get(uploadId));
        }

        protected void Upload(Guid uploadRootId, Guid? entityId, UploadType uploadType, IFormFileCollection files)
        {
            var uploadFiles = files
                .Where(f => f != null)
                .Select(f => uploadFileFactory.Create(uploadRootId, f))
                .ToArray();
            var currentUserId = userProvider.Current.UserId;
            fileUploader.StartUpload(uploadRootId, entityId, currentUserId, uploadType, uploadFiles, ValidationContext, MaxFilesCount, AfterUpload);
        }

        protected virtual int MaxFilesCount => int.MaxValue;

        protected virtual UploadValidationContext ValidationContext => new UploadValidationContext();

        protected virtual void AfterUpload(Guid uploadId, Guid uploadRootId, Guid? entityId) {}
    }
}
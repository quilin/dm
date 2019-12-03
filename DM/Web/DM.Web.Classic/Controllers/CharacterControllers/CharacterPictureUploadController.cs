using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers.CharacterControllers
{
    public class CharacterPictureUploadController : UploadControllerBase
    {
        public CharacterPictureUploadController(
            IFileUploader fileUploader,
            IProgressHandler progressHandler,
            IUploadValidationOptionFactory validationOptionFactory,
            IUploadFileFactory uploadFileFactory,
            IUserProvider userProvider
        ) : base(fileUploader, progressHandler, validationOptionFactory, uploadFileFactory, userProvider)
        {
        }

        public IActionResult GetForm(Guid characterId)
        {
            return PartialView("CharacterPictureUpload/UploadForm", characterId);
        }

        public void Upload(IFormFileCollection files, Guid uploadRootId, Guid characterId)
        {
            Upload(uploadRootId, characterId, UploadType.Public, files);
        }

        protected override int MaxFilesCount => 1;

        protected override UploadValidationContext ValidationContext => base.ValidationContext
            .Add(ValidationOptionFactory.CreateMaxSizeOption(5 * 1 << 20, "Размер файла должен быть меньше 5Мб!"))
            .Add(ValidationOptionFactory.CreateAllowedTypesOption(UploadFileType.Image, "Файл должен быть изображением!"));
    }
}
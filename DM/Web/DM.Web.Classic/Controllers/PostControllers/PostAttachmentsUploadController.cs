namespace DM.Web.Classic.Controllers.PostControllers
{
    public class PostAttachmentsUploadController : UploadControllerBase
    {
        public PostAttachmentsUploadController(
            IFileUploader fileUploader,
            IProgressHandler progressHandler,
            IUploadValidationOptionFactory validationOptionFactory,
            IUploadFileFactory uploadFileFactory,
            IUserProvider userProvider
        ) : base(fileUploader, progressHandler, validationOptionFactory, uploadFileFactory, userProvider)
        {
        }

        protected override int MaxFilesCount => 5;

        protected override UploadValidationContext ValidationContext => base.ValidationContext
            // .Add(ValidationOptionFactory.CreateForbiddenTypesOption()); todo: forbid executables
            .Add(ValidationOptionFactory.CreateMaxSizeOption(10*1 << 20, "Файл должен быть меньше 10Мб"));
    }
}
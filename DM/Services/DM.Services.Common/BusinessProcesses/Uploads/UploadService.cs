using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Dto;
using FluentValidation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <inheritdoc />
    public class UploadService : IUploadService
    {
        private readonly IValidator<CreateUpload> validator;
        private readonly IUploadNameGenerator nameGenerator;
        private readonly IUploadFactory factory;
        private readonly IUploadRepository repository;
        private readonly IUploader uploader;
        private readonly IIdentityProvider identityProvider;

        /// <inheritdoc />
        public UploadService(
            IValidator<CreateUpload> validator,
            IUploadNameGenerator nameGenerator,
            IUploadFactory factory,
            IUploadRepository repository,
            IUploader uploader,
            IIdentityProvider identityProvider)
        {
            this.validator = validator;
            this.nameGenerator = nameGenerator;
            this.factory = factory;
            this.repository = repository;
            this.uploader = uploader;
            this.identityProvider = identityProvider;
        }

        /// <inheritdoc />
        public async Task<Upload> Upload(CreateUpload createUpload)
        {
            await validator.ValidateAndThrowAsync(createUpload);
            var (name, extension) = nameGenerator.Generate(createUpload);

            var filePath = await uploader.Upload(createUpload.StreamAccessor, $"{name}.{extension}");
            var upload = factory.Create(createUpload, filePath, identityProvider.Current.User.UserId, true);

            return await repository.Create(upload);
        }

        private static readonly Size MediumSize = new Size(200, 200);
        private static readonly Size SmallSize = new Size(100, 100);

        /// <inheritdoc />
        public async Task<ImageUploadResult> UploadAndCropImage(CreateUpload createUpload)
        {
            await validator.ValidateAndThrowAsync(createUpload);
            var (name, extension) = nameGenerator.Generate(createUpload);

            using var image = await Image.LoadAsync(createUpload.StreamAccessor());

            var cropRectangle = image.Height > image.Width
                ? new Rectangle(0, (image.Height - image.Width) / 2, image.Width, image.Width)
                : new Rectangle((image.Width - image.Height) / 2, 0, image.Height, image.Height);

            await using var mediumImageStream = new MemoryStream();
            await image.Clone(c => c.Crop(cropRectangle).Resize(MediumSize)).SaveAsJpegAsync(mediumImageStream);

            await using var smallImageStream = new MemoryStream();
            await image.Clone(c => c.Crop(cropRectangle).Resize(SmallSize)).SaveAsJpegAsync(smallImageStream);

            var originalPath = await uploader.Upload(createUpload.StreamAccessor, $"{name}.{extension}");
            var mediumPath = await uploader.Upload(() => mediumImageStream, $"{name}_m.jpg");
            var smallPath = await uploader.Upload(() => smallImageStream, $"{name}_s.jpg");
            
            var userId = identityProvider.Current.User.UserId;
            var uploads = await repository.Create(new[] {originalPath, mediumPath, smallPath}
                .Select(path => factory.Create(createUpload, path, userId, path == originalPath)));

            return new ImageUploadResult
            {
                OriginalFilePath = originalPath,
                MediumCroppedFilePath = mediumPath,
                SmallCroppedFilePath = smallPath,
                Uploads = uploads
            };
        }
    }
}
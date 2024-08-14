using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Implementation;
using DM.Services.Uploading.BusinessProcesses.Cdn;
using DM.Services.Uploading.BusinessProcesses.Shared;
using DM.Services.Uploading.Dto;
using FluentValidation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DM.Services.Uploading.BusinessProcesses.PublicImage;

/// <inheritdoc />
internal class PublicImageService : IPublicImageService
{
    private readonly IValidator<CreateUpload> validator;
    private readonly INameGenerator nameGenerator;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IUploader uploader;
    private readonly IUploadFactory factory;
    private readonly IPublicImageUploadRepository repository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public PublicImageService(
        IValidator<CreateUpload> validator,
        INameGenerator nameGenerator,
        IDateTimeProvider dateTimeProvider,
        IUploader uploader,
        IUploadFactory factory,
        IPublicImageUploadRepository repository,
        IIdentityProvider identityProvider)
    {
        this.validator = validator;
        this.nameGenerator = nameGenerator;
        this.dateTimeProvider = dateTimeProvider;
        this.uploader = uploader;
        this.factory = factory;
        this.repository = repository;
        this.identityProvider = identityProvider;
    }

    private static readonly Size MediumSize = new(200, 200);
    private static readonly Size SmallSize = new(100, 100);

    /// <inheritdoc />
    public async Task<(Upload original, Upload medium, Upload small)> Upload(CreateUpload createUpload)
    {
        await validator.ValidateAndThrowAsync(createUpload);
        var (name, extension) = await nameGenerator.Generate(createUpload);

        using var image = await Image.LoadAsync(createUpload.StreamAccessor());
        var cropRectangle = image.Height > image.Width
            ? new Rectangle(0, (image.Height - image.Width) / 2, image.Width, image.Width)
            : new Rectangle((image.Width - image.Height) / 2, 0, image.Height, image.Height);

        await using var mediumImageStream = new MemoryStream();
        await image.Clone(c => c.Crop(cropRectangle).Resize(MediumSize)).SaveAsJpegAsync(mediumImageStream);

        await using var smallImageStream = new MemoryStream();
        await image.Clone(c => c.Crop(cropRectangle).Resize(SmallSize)).SaveAsJpegAsync(smallImageStream);

        var originalImagePath = await uploader.Upload(createUpload.StreamAccessor, $"{name}{extension}");
        var mediumImagePath = await uploader.Upload(() => mediumImageStream, $"{name}_m.jpg");
        var smallImagePath = await uploader.Upload(() => smallImageStream, $"{name}_s.jpg");

        var userId = identityProvider.Current.User.UserId;
        var createdAt = dateTimeProvider.Now;
        var uploads = new[] {originalImagePath, mediumImagePath, smallImagePath}
            .Select(path => factory.Create(createUpload, path, userId, path == originalImagePath, createdAt));

        var uploadsIndex = (await repository.Create(uploads)).ToDictionary(u => u.FilePath);
        return (uploadsIndex[originalImagePath], uploadsIndex[mediumImagePath], uploadsIndex[smallImagePath]);
    }

    /// <inheritdoc />
    public Task PrepareObsoleteForDeleting(Guid entityId) => repository.RemoveObsoleteUploads(entityId);
}
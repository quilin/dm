using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Extensions;
using DM.Services.Core.Implementation;
using FluentValidation;

namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <inheritdoc />
    public class UploadService : IUploadService
    {
        private readonly IValidator<CreateUpload> validator;
        private readonly IUploadFactory factory;
        private readonly IUploadRepository repository;
        private readonly IUploader uploader;
        private readonly IGuidFactory guidFactory;
        private readonly IIdentityProvider identityProvider;

        /// <inheritdoc />
        public UploadService(
            IValidator<CreateUpload> validator,
            IUploadFactory factory,
            IUploadRepository repository,
            IUploader uploader,
            IGuidFactory guidFactory,
            IIdentityProvider identityProvider)
        {
            this.validator = validator;
            this.factory = factory;
            this.repository = repository;
            this.uploader = uploader;
            this.guidFactory = guidFactory;
            this.identityProvider = identityProvider;
        }

        /// <inheritdoc />
        public async Task<Upload> Upload(CreateUpload createUpload)
        {
            await validator.ValidateAndThrowAsync(createUpload);
            var fileName = GenerateFileName(createUpload);
            var filePath = await uploader.Upload(createUpload.StreamAccessor, fileName);
            var upload = factory.Create(createUpload, filePath, identityProvider.Current.User.UserId);

            return await repository.Create(upload);
        }

        private static readonly IDictionary<string, string> FileExtensions = new Dictionary<string, string>
        {
            {FileMimeTypeNames.Image.Gif, "gif"},
            {FileMimeTypeNames.Image.Jpeg, "jpg"},
            {FileMimeTypeNames.Image.Pjpeg, "jpg"},
            {FileMimeTypeNames.Image.Png, "png"},
            {FileMimeTypeNames.Image.Svg, "svg"},
            {FileMimeTypeNames.Image.Tiff, "tif"},

            {FileMimeTypeNames.Application.Pdf, "pdf"},
            {FileMimeTypeNames.Application.Zip, "zip"},
            {FileMimeTypeNames.Application.Gzip, "gzip"},

            {FileMimeTypeNames.Text.Plain, "txt"},
            {FileMimeTypeNames.Text.Html, "htm"}
        };

        private string GenerateFileName(CreateUpload createUpload)
        {
            var extension = FileExtensions.TryGetValue(createUpload.ContentType, out var predefinedExtension)
                ? $".{predefinedExtension}"
                : Path.GetExtension(createUpload.FileName);
            var originalNameHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(createUpload.FileName));
            var salt = Convert.ToBase64String(guidFactory.Create().ToByteArray());
            var fileName = Regex.Replace(originalNameHash + salt, @"\W", string.Empty);

            return $"{fileName}{extension}";
        }
    }
}
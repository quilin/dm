using System;
using DM.Services.Core.Implementation;
using DbUpload = DM.Services.DataAccess.BusinessObjects.Common.Upload;

namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <inheritdoc />
    public class UploadFactory : IUploadFactory
    {
        private readonly IGuidFactory guidFactory;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public UploadFactory(
            IGuidFactory guidFactory,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidFactory = guidFactory;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <inheritdoc />
        public DbUpload Create(CreateUpload createUpload, string filePath, Guid userId, bool original) => new DbUpload
        {
            UploadId = guidFactory.Create(),
            CreateDate = dateTimeProvider.Now,
            UserId = userId,
            EntityId = createUpload.EntityId,
            FileName = createUpload.FileName,
            FilePath = filePath,
            Original = original,
            IsRemoved = false
        };
    }
}
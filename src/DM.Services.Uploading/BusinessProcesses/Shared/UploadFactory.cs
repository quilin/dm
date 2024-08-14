using System;
using DM.Services.Core.Implementation;
using DM.Services.Uploading.Dto;
using DbUpload = DM.Services.DataAccess.BusinessObjects.Common.Upload;

namespace DM.Services.Uploading.BusinessProcesses.Shared;

/// <inheritdoc />
internal class UploadFactory : IUploadFactory
{
    private readonly IGuidFactory guidFactory;

    /// <inheritdoc />
    public UploadFactory(
        IGuidFactory guidFactory)
    {
        this.guidFactory = guidFactory;
    }

    /// <inheritdoc />
    public DbUpload Create(CreateUpload createUpload, string filePath, Guid userId, bool original,
        DateTimeOffset createdAt) => new()
    {
        UploadId = guidFactory.Create(),
        CreateDate = createdAt,
        UserId = userId,
        EntityId = createUpload.EntityId,
        FileName = createUpload.FileName,
        FilePath = filePath,
        Original = original,
        IsRemoved = false
    };
}
using System;
using DM.Services.Uploading.Dto;
using DbUpload = DM.Services.DataAccess.BusinessObjects.Common.Upload;

namespace DM.Services.Uploading.BusinessProcesses.Shared;

/// <summary>
/// Factory for DAL upload model
/// </summary>
internal interface IUploadFactory
{
    /// <summary>
    /// Create new upload DAL
    /// </summary>
    /// <param name="createUpload"></param>
    /// <param name="filePath"></param>
    /// <param name="userId"></param>
    /// <param name="original"></param>
    /// <param name="createdAt"></param>
    /// <returns></returns>
    DbUpload Create(CreateUpload createUpload, string filePath, Guid userId, bool original, DateTimeOffset createdAt);
}
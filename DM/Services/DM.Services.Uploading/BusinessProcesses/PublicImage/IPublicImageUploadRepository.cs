using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Uploading.Dto;
using DbUpload = DM.Services.DataAccess.BusinessObjects.Common.Upload;

namespace DM.Services.Uploading.BusinessProcesses.PublicImage;

/// <summary>
/// Storage for public image uploads
/// </summary>
internal interface IPublicImageUploadRepository
{
    /// <summary>
    /// Save image uploads
    /// </summary>
    /// <param name="uploads"></param>
    /// <returns></returns>
    Task<IEnumerable<Upload>> Create(IEnumerable<DbUpload> uploads);

    /// <summary>
    /// Mark obsolete (all but recently added) uploads for deleting
    /// </summary>
    /// <param name="entityId">Entity identifier</param>
    /// <returns></returns>
    Task RemoveObsoleteUploads(Guid entityId);
}
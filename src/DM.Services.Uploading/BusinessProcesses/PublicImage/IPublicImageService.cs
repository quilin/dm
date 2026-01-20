using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Uploading.Dto;

namespace DM.Services.Uploading.BusinessProcesses.PublicImage;

/// <summary>
/// Service for uploading images for display on website
/// </summary>
public interface IPublicImageService
{
    /// <summary>
    /// Upload original image and also its cropped it and resized to medium and small size versions
    /// </summary>
    /// <param name="createUpload"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<(Upload original, Upload medium, Upload small)> Upload(CreateUpload createUpload,
        CancellationToken cancellationToken);

    /// <summary>
    /// Prepare obsolete images for deleting
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PrepareObsoleteForDeleting(Guid entityId, CancellationToken cancellationToken);
}
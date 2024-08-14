using System.Threading.Tasks;
using DM.Services.Uploading.Dto;

namespace DM.Services.Uploading.BusinessProcesses.Attachment;

/// <summary>
/// Service for uploading and storing meta-information about uploads
/// </summary>
public interface IAttachmentUploadService
{
    /// <summary>
    /// Upload new file
    /// </summary>
    /// <param name="createUpload"></param>
    /// <returns></returns>
    Task<Upload> Create(CreateUpload createUpload);
}
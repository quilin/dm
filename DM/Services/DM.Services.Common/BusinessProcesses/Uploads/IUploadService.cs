using System.Threading.Tasks;
using DM.Services.Common.Dto;

namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <summary>
    /// Service for file uploads
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// Upload new file
        /// </summary>
        /// <returns></returns>
        Task<Upload> Upload(CreateUpload createUpload);

        /// <summary>
        /// Upload image for display with cropping and resizing it
        /// </summary>
        /// <returns></returns>
        Task<ImageUploadResult> UploadAndCropImage(CreateUpload createUpload);
    }
}
using System.Threading.Tasks;

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
    }
}
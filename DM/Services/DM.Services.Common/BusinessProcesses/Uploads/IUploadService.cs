using System.Threading.Tasks;
using DM.Services.Common.Dto;

namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <summary>
    /// Service for operating user uploads
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createUpload"></param>
        /// <returns></returns>
        Task<Upload> Create(CreateUpload createUpload);
    }
}
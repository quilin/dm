using System.Collections.Generic;
using System.Threading.Tasks;
using DbUpload = DM.Services.DataAccess.BusinessObjects.Common.Upload;

namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <summary>
    /// Storage for uploads information
    /// </summary>
    public interface IUploadRepository
    {
        /// <summary>
        /// Save upload
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        Task<Upload> Create(DbUpload upload);

        /// <summary>
        /// Save uploads
        /// </summary>
        /// <param name="uploads"></param>
        /// <returns></returns>
        Task<IEnumerable<Upload>> Create(IEnumerable<DbUpload> uploads);
    }
}
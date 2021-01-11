using System;
using DbUpload = DM.Services.DataAccess.BusinessObjects.Common.Upload;

namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <summary>
    /// Factory for DAL upload model
    /// </summary>
    public interface IUploadFactory
    {
        /// <summary>
        /// Create new upload DAL
        /// </summary>
        /// <param name="createUpload"></param>
        /// <param name="filePath"></param>
        /// <param name="userId"></param>
        /// <param name="original"></param>
        /// <returns></returns>
        DbUpload Create(CreateUpload createUpload, string filePath, Guid userId, bool original);
    }
}
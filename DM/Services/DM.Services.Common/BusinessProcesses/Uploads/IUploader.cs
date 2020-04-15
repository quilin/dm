using System;
using System.IO;
using System.Threading.Tasks;

namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <summary>
    /// File uploader
    /// </summary>
    public interface IUploader
    {
        /// <summary>
        /// Upload file to the file storage
        /// </summary>
        /// <param name="fileStream">File content stream</param>
        /// <param name="fileName">File name</param>
        /// <returns>File path</returns>
        Task<string> Upload(Func<Stream> fileStream, string fileName);
    }
}
using System;
using System.IO;

namespace DM.Services.Common.Dto
{
    /// <summary>
    /// DTO for new upload
    /// </summary>
    public class CreateUpload
    {
        /// <summary>
        /// Parent entity identifier
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// File name for display
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Incoming file stream
        /// </summary>
        public Stream FileStream { get; set; }
    }
}
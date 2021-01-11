using System.Collections.Generic;
using DM.Services.Common.BusinessProcesses.Uploads;

namespace DM.Services.Common.Dto
{
    /// <summary>
    /// DTO model for cropped and resized image upload
    /// </summary>
    public class ImageUploadResult
    {
        /// <summary>
        /// File path for original image
        /// </summary>
        public string OriginalFilePath { get; set; }

        /// <summary>
        /// File path for medium-sized image
        /// </summary>
        public string MediumCroppedFilePath { get; set; }

        /// <summary>
        /// File path for small-sized image
        /// </summary>
        public string SmallCroppedFilePath { get; set; }

        /// <summary>
        /// Created uploads
        /// </summary>
        public IEnumerable<Upload> Uploads { get; set; }
    }
}
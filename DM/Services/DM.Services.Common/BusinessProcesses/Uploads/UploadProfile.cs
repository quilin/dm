using AutoMapper;
using DbUpload = DM.Services.DataAccess.BusinessObjects.Common.Upload;

namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <summary>
    /// Profile for upload mapper
    /// </summary>
    public class UploadProfile : Profile
    {
        /// <inheritdoc />
        public UploadProfile()
        {
            CreateMap<DbUpload, Upload>();
        }
    }
}
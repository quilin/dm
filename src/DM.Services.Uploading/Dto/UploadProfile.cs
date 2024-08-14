using AutoMapper;
using DbUpload = DM.Services.DataAccess.BusinessObjects.Common.Upload;

namespace DM.Services.Uploading.Dto;

/// <summary>
/// Profile for upload mapper
/// </summary>
internal class UploadProfile : Profile
{
    /// <inheritdoc />
    public UploadProfile()
    {
        CreateMap<DbUpload, Upload>();
    }
}
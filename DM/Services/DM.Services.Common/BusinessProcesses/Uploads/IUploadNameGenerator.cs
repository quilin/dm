namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <summary>
    /// Upload name generator for storage
    /// </summary>
    public interface IUploadNameGenerator
    {
        /// <summary>
        /// Generate name and file extension for the upload
        /// </summary>
        /// <param name="createUpload"></param>
        /// <returns></returns>
        (string name, string extension) Generate(CreateUpload createUpload);
    }
}
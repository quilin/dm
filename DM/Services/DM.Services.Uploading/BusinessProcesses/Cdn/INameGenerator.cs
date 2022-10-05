using System.Threading.Tasks;
using DM.Services.Uploading.Dto;

namespace DM.Services.Uploading.BusinessProcesses.Cdn;

/// <summary>
/// Upload name generator
/// </summary>
internal interface INameGenerator
{
    /// <summary>
    /// Generate upload name and extension
    /// </summary>
    /// <param name="createUpload"></param>
    /// <returns></returns>
    Task<(string name, string extension)> Generate(CreateUpload createUpload);
}
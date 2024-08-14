using System.Threading.Tasks;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Deleting;

/// <summary>
/// Service for deleting blacklist links
/// </summary>
public interface IBlacklistDeletingService
{
    /// <summary>
    /// Delete existing blacklist link
    /// </summary>
    /// <param name="operateBlacklistLink">DTO model</param>
    /// <returns></returns>
    Task Delete(OperateBlacklistLink operateBlacklistLink);
}
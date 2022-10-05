using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Creating;

/// <summary>
/// Service for blacklist creating
/// </summary>
public interface IBlacklistCreatingService
{
    /// <summary>
    /// Create new blacklist link
    /// </summary>
    /// <param name="operateBlacklistLink">DTO for creating</param>
    /// <returns></returns>
    Task<GeneralUser> Create(OperateBlacklistLink operateBlacklistLink);
}
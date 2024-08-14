using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.DataAccess.BusinessObjects.Games.Links;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Creating;

/// <summary>
/// Storage for blacklist link creating
/// </summary>
internal interface IBlacklistCreatingRepository
{
    /// <summary>
    /// Save new link
    /// </summary>
    /// <param name="link">DAL model</param>
    /// <returns></returns>
    Task<GeneralUser> Create(BlackListLink link);
}
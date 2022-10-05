using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Games.Links;
using DM.Services.DataAccess.RelationalStorage;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Deleting;

/// <summary>
/// Storage for blacklist link deleting
/// </summary>
internal interface IBlacklistDeletingRepository
{
    /// <summary>
    /// Delete existing blacklist link
    /// </summary>
    /// <param name="updateBuilder"></param>
    /// <returns></returns>
    Task Delete(IUpdateBuilder<BlackListLink> updateBuilder);
}
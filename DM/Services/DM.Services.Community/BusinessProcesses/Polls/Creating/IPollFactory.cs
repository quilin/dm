using DM.Services.DataAccess.BusinessObjects.Fora;

namespace DM.Services.Community.BusinessProcesses.Polls.Creating;

/// <summary>
/// Factory for DAL poll model
/// </summary>
internal interface IPollFactory
{
    /// <summary>
    /// Create new poll DAL model
    /// </summary>
    /// <param name="createPoll"></param>
    /// <returns></returns>
    Poll Create(CreatePoll createPoll);
}
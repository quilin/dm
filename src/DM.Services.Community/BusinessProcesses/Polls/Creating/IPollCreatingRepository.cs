using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DbPoll = DM.Services.DataAccess.BusinessObjects.Fora.Poll;

namespace DM.Services.Community.BusinessProcesses.Polls.Creating;

/// <summary>
/// Storage for poll creating
/// </summary>
internal interface IPollCreatingRepository
{
    /// <summary>
    /// Create new poll
    /// </summary>
    /// <param name="poll"></param>
    /// <returns></returns>
    Task<Poll> Create(DbPoll poll);
}
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Polls.Reading;

namespace DM.Services.Community.BusinessProcesses.Polls.Creating;

/// <summary>
/// Service for poll creating
/// </summary>
public interface IPollCreatingService
{
    /// <summary>
    /// Create new poll and start immediately
    /// </summary>
    /// <param name="createPoll"></param>
    /// <returns></returns>
    Task<Poll> Create(CreatePoll createPoll);
}
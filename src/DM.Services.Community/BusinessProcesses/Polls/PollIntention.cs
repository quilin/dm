namespace DM.Services.Community.BusinessProcesses.Polls;

/// <summary>
/// List of poll actions that require authorization
/// </summary>
public enum PollIntention
{
    /// <summary>
    /// Create new polls
    /// </summary>
    Create = 0,

    /// <summary>
    /// Take a vote in an active poll
    /// </summary>
    Vote = 1
}
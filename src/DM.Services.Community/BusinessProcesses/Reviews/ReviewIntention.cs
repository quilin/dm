namespace DM.Services.Community.BusinessProcesses.Reviews;

/// <summary>
/// List of review actions that require authorization
/// </summary>
public enum ReviewIntention
{
    /// <summary>
    /// Create new review
    /// </summary>
    Create = 0,

    /// <summary>
    /// Edit existing review
    /// </summary>
    Edit = 1,

    /// <summary>
    /// Approve review
    /// </summary>
    Approve = 2,

    /// <summary>
    /// Delete review
    /// </summary>
    Delete = 3,

    /// <summary>
    /// Read not approved reviews
    /// </summary>
    ReadUnapproved = 4,
}
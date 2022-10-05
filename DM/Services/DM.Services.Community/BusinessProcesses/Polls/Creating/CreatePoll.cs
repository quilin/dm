using System;
using System.Collections.Generic;

namespace DM.Services.Community.BusinessProcesses.Polls.Creating;

/// <summary>
/// DTO model for poll creating
/// </summary>
public class CreatePoll
{
    /// <summary>
    /// Poll title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Desired poll end date
    /// </summary>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// List of possible poll answers
    /// </summary>
    public IEnumerable<string> Options { get; set; }
}
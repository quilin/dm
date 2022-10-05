using System;
using System.Collections.Generic;

namespace DM.Services.Community.BusinessProcesses.Polls.Reading;

/// <summary>
/// DTO model for poll
/// </summary>
public class Poll
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Start moment
    /// </summary>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// End moment
    /// </summary>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Question text
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Answers list
    /// </summary>
    public IEnumerable<PollOption> Options { get; set; }
}

/// <summary>
/// DTO model for poll answer option
/// </summary>
public class PollOption
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Answer text
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// List of voted users
    /// </summary>
    public IEnumerable<Guid> UserIds { get; set; }
}
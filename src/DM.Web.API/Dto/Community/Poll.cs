using System;
using System.Collections.Generic;

namespace DM.Web.API.Dto.Community;

/// <summary>
/// API model for poll
/// </summary>
public class Poll
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// End date
    /// </summary>
    public DateTimeOffset Ends { get; set; }

    /// <summary>
    /// Poll question
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Poll answer options
    /// </summary>
    public IEnumerable<PollOption> Options { get; set; }
}

/// <summary>
/// API model for poll option
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
    /// Votes given for the answer
    /// </summary>
    public int VotesCount { get; set; }

    /// <summary>
    /// Current user has voted for it
    /// </summary>
    public bool? Voted { get; set; }
}
using System;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Reviews.Reading;

/// <summary>
/// DTO model for review
/// </summary>
public class Review
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Review author
    /// </summary>
    public GeneralUser Author { get; set; }

    /// <summary>
    /// Creating moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Review is published
    /// </summary>
    public bool Approved { get; set; }

    /// <summary>
    /// Review text
    /// </summary>
    public string Text { get; set; }
}
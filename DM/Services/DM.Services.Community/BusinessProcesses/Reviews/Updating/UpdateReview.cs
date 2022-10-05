using System;

namespace DM.Services.Community.BusinessProcesses.Reviews.Updating;

/// <summary>
/// DTO model 
/// </summary>
public class UpdateReview
{
    /// <summary>
    /// Review identifier
    /// </summary>
    public Guid ReviewId { get; set; }
        
    /// <summary>
    /// Review text
    /// </summary>
    public string Text { get; set; }
        
    /// <summary>
    /// Approval flag
    /// </summary>
    public bool? Approved { get; set; }
}
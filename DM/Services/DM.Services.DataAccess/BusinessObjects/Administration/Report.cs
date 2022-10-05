using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Administration;

/// <summary>
/// DAL model of user complaint
/// </summary>
[Table("Reports")]
public class Report
{
    /// <summary>
    /// Report identifier
    /// </summary>
    [Key]
    public Guid ReportId { get; set; }

    /// <summary>
    /// Complaint author identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Complaint target user identifier
    /// </summary>
    public Guid TargetId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Causation entity text
    /// </summary>
    public string ReportText { get; set; }

    /// <summary>
    /// Reporter comment
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// Complaint responsible identifier
    /// </summary>
    public Guid? AnswerAuthorId { get; set; }

    /// <summary>
    /// Complaint responsible commentary
    /// </summary>
    public string Answer { get; set; }

    /// <summary>
    /// Complaint author
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User Author { get; set; }

    /// <summary>
    /// Complaint target user
    /// </summary>
    [ForeignKey(nameof(TargetId))]
    public virtual User Target { get; set; }

    /// <summary>
    /// Complaint responsible
    /// </summary>
    [ForeignKey(nameof(AnswerAuthorId))]
    public virtual User AnswerAuthor { get; set; }
}
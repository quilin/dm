using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.DataAccess.BusinessObjects.Common;

/// <summary>
/// DAL model for user review
/// </summary>
[Table("Reviews")]
public class Review : IRemovable
{
    /// <summary>
    /// Review identifier
    /// </summary>
    [Key]
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Author identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Review content
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Premoderation flag
    /// </summary>
    public bool IsApproved { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <summary>
    /// Author
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User Author { get; set; }

    /// <summary>
    /// Likes
    /// </summary>
    [InverseProperty(nameof(Like.Review))]
    public virtual ICollection<Like> Likes { get; set; }
}
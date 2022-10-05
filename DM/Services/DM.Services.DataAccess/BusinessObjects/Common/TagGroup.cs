using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DM.Services.DataAccess.BusinessObjects.Common;

/// <summary>
/// DAL model for tag group
/// </summary>
[Table("TagGroups")]
public class TagGroup
{
    /// <summary>
    /// Tag group identifier
    /// </summary>
    [Key]
    public Guid TagGroupId { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Tags under the group
    /// </summary>
    [InverseProperty(nameof(Tag.TagGroup))]
    public virtual ICollection<Tag> Tags { get; set; }
}
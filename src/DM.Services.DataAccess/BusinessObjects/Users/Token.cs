using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DM.Services.DataAccess.BusinessObjects.DataContracts;
using DM.Services.DataAccess.BusinessObjects.Games;

namespace DM.Services.DataAccess.BusinessObjects.Users;

/// <summary>
/// DAL model for authorization token
/// </summary>
public class Token : IRemovable
{
    /// <summary>
    /// Token identifier
    /// </summary>
    [Key]
    public Guid TokenId { get; set; }

    /// <summary>
    /// Authorised user identifier
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Related entity identifier
    /// </summary>
    public Guid? EntityId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    public DateTimeOffset CreateDate { get; set; }

    /// <summary>
    /// Authorised action type
    /// </summary>
    public TokenType Type { get; set; }

    /// <inheritdoc />
    public bool IsRemoved { get; set; }

    /// <summary>
    /// Authorised user
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    /// <summary>
    /// Related game
    /// </summary>
    [ForeignKey(nameof(EntityId))]
    public virtual Game Game { get; set; }
}
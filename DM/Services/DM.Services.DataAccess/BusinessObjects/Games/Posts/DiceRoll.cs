using System;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Bson.Serialization.Attributes;

namespace DM.Services.DataAccess.BusinessObjects.Games.Posts;

/// <summary>
/// DAL model for dice roll
/// </summary>
[MongoCollectionName("Dice")]
public class DiceRoll
{
    /// <summary>
    /// Roll identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Post identifier
    /// </summary>
    public Guid PostId { get; set; }

    /// <summary>
    /// Creation moment
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Is appended flag
    /// </summary>
    public bool IsAdditional { get; set; }

    /// <summary>
    /// Only GM and post author can see hidden rolls
    /// </summary>
    public bool IsHidden { get; set; }

    /// <summary>
    /// Fair roll result can only be seen after the post was created
    /// </summary>
    public bool IsFair { get; set; }

    /// <summary>
    /// Number of dice (X in XdY)
    /// </summary>
    public int DiceCount { get; set; }

    /// <summary>
    /// Number of single die edges (Y in XdY)
    /// </summary>
    public int EdgesCount { get; set; }

    /// <summary>
    /// Maximum number of dice blast (no blast if 0, any number of blast if null)
    /// </summary>
    public int? BlastCount { get; set; }

    /// <summary>
    /// Constant bonus
    /// </summary>
    public int Bonus { get; set; }

    /// <summary>
    /// Roll commentary
    /// </summary>
    public string Commentary { get; set; }

    /// <summary>
    /// Result
    /// </summary>
    public RollResult[] Result { get; set; }
}

/// <summary>
/// DAL model for a single die roll result
/// </summary>
public class RollResult
{
    /// <summary>
    /// Die value
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Critical flag
    /// </summary>
    public bool IsCritical { get; set; }

    /// <summary>
    /// Blasted flag
    /// </summary>
    public bool IsBlasted { get; set; }
}
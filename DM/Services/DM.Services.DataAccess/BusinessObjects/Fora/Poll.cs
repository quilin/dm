using System;
using System.Collections.Generic;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Bson.Serialization.Attributes;

namespace DM.Services.DataAccess.BusinessObjects.Fora;

/// <summary>
/// DAL model for poll
/// </summary>
[MongoCollectionName("Polls")]
public class Poll
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Moment from
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Moment to
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Poll is global
    /// </summary>
    public bool Global { get; set; }

    /// <summary>
    /// Question text
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Options
    /// </summary>
    public List<PollOption> Options { get; set; }
}

/// <summary>
/// DAL model for poll option
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
    /// Voted users identifiers
    /// </summary>
    public List<Guid> UserIds { get; set; }
}
using System;
using System.Collections.Generic;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Bson.Serialization.Attributes;

namespace DM.Services.DataAccess.BusinessObjects.Users;

/// <summary>
/// DAL model for user authentication
/// </summary>
[MongoCollectionName("UserSessions")]
public class UserSessions
{
    /// <summary>
    /// User identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Authentication sessions
    /// </summary>
    public List<Session> Sessions { get; set; }
}

/// <summary>
/// DAL model for authentication session
/// </summary>
public class Session
{
    /// <summary>
    /// Session identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Expiration moment
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    /// Persistence flag
    /// </summary>
    public bool Persistent { get; set; }

    /// <summary>
    /// Flag of invisible log in
    /// </summary>
    public bool Invisible { get; set; }
}
using System;
using System.ComponentModel.DataAnnotations;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Bson.Serialization.Attributes;

namespace DM.Services.DataAccess.BusinessObjects.Users.Settings;

/// <summary>
/// DAL model for user settings
/// </summary>
[MongoCollectionName("UserSettings")]
[BsonIgnoreExtraElements]
public class UserSettings
{
    /// <summary>
    /// User identifier
    /// </summary>
    [Key]
    public Guid UserId { get; set; }

    /// <summary>
    /// Paging settings
    /// </summary>
    public PagingSettings Paging { get; set; }

    /// <summary>
    /// Message that user's newbies will receive once they are connected
    /// </summary>
    public string NannyGreetingsMessage { get; set; }

    /// <summary>
    /// Website color scheme
    /// </summary>
    public ColorSchema ColorSchema { get; set; }
}
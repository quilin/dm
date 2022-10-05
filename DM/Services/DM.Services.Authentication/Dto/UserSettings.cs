using System;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Authentication.Dto;

/// <summary>
/// User settings
/// </summary>
public class UserSettings
{
    /// <summary>
    /// Settings id (should be same as UserId)
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// If the user is nanny, each newbie linked to it will receive the custom private message upon registration
    /// </summary>
    public string NannyGreetingsMessage { get; set; }

    /// <summary>
    /// Color scheme for the website view
    /// </summary>
    public ColorSchema ColorSchema { get; set; }

    /// <summary>
    /// Paging settings
    /// </summary>
    public PagingSettings Paging { get; set; }

    /// <summary>
    /// Default user settings for a guest or a newbie
    /// </summary>
    public static readonly UserSettings Default = new()
    {
        Paging = new PagingSettings
        {
            TopicsPerPage = 10,
            CommentsPerPage = 10,
            PostsPerPage = 10,
            MessagesPerPage = 10,
            EntitiesPerPage = 10
        },
        ColorSchema = ColorSchema.Modern
    };
}
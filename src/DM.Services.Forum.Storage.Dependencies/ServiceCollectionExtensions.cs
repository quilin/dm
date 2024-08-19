using AutoMapper;
using DM.Services.Forum.BusinessProcesses.Commentaries.Creating;
using DM.Services.Forum.BusinessProcesses.Commentaries.Deleting;
using DM.Services.Forum.BusinessProcesses.Commentaries.Reading;
using DM.Services.Forum.BusinessProcesses.Commentaries.Updating;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.BusinessProcesses.Moderation;
using DM.Services.Forum.BusinessProcesses.Topics.Creating;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.BusinessProcesses.Topics.Updating;
using DM.Services.Forum.Storage.Profiles;
using DM.Services.Forum.Storage.Storages.Commentaries;
using DM.Services.Forum.Storage.Storages.Fora;
using DM.Services.Forum.Storage.Storages.Topics;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Forum.Storage.Dependencies;

/// <summary>
/// Registrar
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register forum storage dependencies
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddForumStorage(this IServiceCollection services)
    {
        services.AddScoped<IForumRepository, ForumRepository>();
        services.AddScoped<IModeratorRepository, ModeratorRepository>();

        services.AddScoped<ITopicCreatingRepository, TopicCreatingRepository>();
        services.AddScoped<ITopicReadingRepository, TopicReadingRepository>();
        services.AddScoped<ITopicUpdatingRepository, TopicUpdatingRepository>();

        services.AddScoped<ICommentaryCreatingRepository, CommentaryCreatingRepository>();
        services.AddScoped<ICommentaryReadingRepository, CommentaryReadingRepository>();
        services.AddScoped<ICommentaryUpdatingRepository, CommentaryUpdatingRepository>();
        services.AddScoped<ICommentaryDeletingRepository, CommentaryDeletingRepository>();

        // TODO: Switch to .AddAutoMapper
        services.AddScoped<Profile, ForumProfile>();
        services.AddScoped<Profile, TopicProfile>();

        return services;
    }
}
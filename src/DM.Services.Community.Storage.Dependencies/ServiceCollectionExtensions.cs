using AutoMapper;
using DM.Services.Community.BusinessProcesses.Account.Activation;
using DM.Services.Community.BusinessProcesses.Account.EmailChange;
using DM.Services.Community.BusinessProcesses.Account.PasswordChange;
using DM.Services.Community.BusinessProcesses.Account.PasswordReset;
using DM.Services.Community.BusinessProcesses.Account.Registration;
using DM.Services.Community.BusinessProcesses.Account.Verification;
using DM.Services.Community.BusinessProcesses.Chat.Creating;
using DM.Services.Community.BusinessProcesses.Chat.Reading;
using DM.Services.Community.BusinessProcesses.Messaging.Creating;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.Community.BusinessProcesses.Polls.Creating;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.Community.BusinessProcesses.Polls.Voting;
using DM.Services.Community.BusinessProcesses.Reviews.Creating;
using DM.Services.Community.BusinessProcesses.Reviews.Reading;
using DM.Services.Community.BusinessProcesses.Reviews.Updating;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Community.BusinessProcesses.Users.Updating;
using DM.Services.Community.Storage.Profiles;
using DM.Services.Community.Storage.Storages.Account;
using DM.Services.Community.Storage.Storages.Messaging;
using DM.Services.Community.Storage.Storages.Polls;
using DM.Services.Community.Storage.Storages.Reviews;
using DM.Services.Community.Storage.Storages.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DM.Services.Community.Storage.Dependencies;

/// <summary>
/// Registrar
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register community storage dependencies
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCommunityStorage(this IServiceCollection services)
    {
        services.AddScoped<IActivationRepository, ActivationRepository>();
        services.AddScoped<IEmailChangeRepository, EmailChangeRepository>();
        services.AddScoped<IPasswordChangeRepository, PasswordChangeRepository>();
        services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        services.AddScoped<ITokenVerificationRepository, TokenVerificationRepository>();

        services.AddScoped<IChatCreatingRepository, ChatCreatingRepository>();
        services.AddScoped<IChatReadingRepository, ChatReadingRepository>();
        services.AddScoped<IConversationReadingRepository, ConversationReadingRepository>();
        services.AddScoped<IMessageCreatingRepository, MessageCreatingRepository>();
        services.AddScoped<IMessageReadingRepository, MessageReadingRepository>();

        services.AddScoped<IPollCreatingRepository, PollCreatingRepository>();
        services.AddScoped<IPollReadingRepository, PollReadingRepository>();
        services.AddScoped<IPollVotingRepository, PollVotingRepository>();

        services.AddScoped<IReviewCreatingRepository, ReviewCreatingRepository>();
        services.AddScoped<IReviewReadingRepository, ReviewReadingRepository>();
        services.AddScoped<IReviewUpdatingRepository, ReviewUpdatingRepository>();

        services.AddScoped<IUserReadingRepository, UserReadingRepository>();
        services.AddScoped<IUserUpdatingRepository, UserUpdatingRepository>();

        // TODO: Switch to .AddAutoMapper
        services.AddScoped<Profile, ChatMessageProfile>();
        services.AddScoped<Profile, MessagingProfile>();
        services.AddScoped<Profile, ReadingProfile>();
        services.AddScoped<Profile, ReviewProfile>();
        
        return services;
    }
}
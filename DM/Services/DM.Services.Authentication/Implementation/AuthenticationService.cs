using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using DbSession = DM.Services.DataAccess.BusinessObjects.Users.Session;

namespace DM.Services.Authentication.Implementation;

/// <inheritdoc />
internal class AuthenticationService : IAuthenticationService
{
    private readonly ISecurityManager securityManager;
    private readonly ISymmetricCryptoService cryptoService;
    private readonly IAuthenticationRepository repository;
    private readonly ISessionFactory sessionFactory;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IIdentityProvider identityProvider;
    private readonly IUpdateBuilderFactory updateBuilderFactory;

    private const string UserIdKey = "userId";
    private const string SessionIdKey = "sessionId";

    /// <inheritdoc />
    public AuthenticationService(
        ISecurityManager securityManager,
        ISymmetricCryptoService cryptoService,
        IAuthenticationRepository repository,
        ISessionFactory sessionFactory,
        IDateTimeProvider dateTimeProvider,
        IIdentityProvider identityProvider,
        IUpdateBuilderFactory updateBuilderFactory)
    {
        this.securityManager = securityManager;
        this.cryptoService = cryptoService;
        this.repository = repository;
        this.sessionFactory = sessionFactory;
        this.dateTimeProvider = dateTimeProvider;
        this.identityProvider = identityProvider;
        this.updateBuilderFactory = updateBuilderFactory;
    }

    /// <inheritdoc />
    public async Task<IIdentity> Authenticate(string login, string password, bool persistent)
    {
        var (userFound, user) = await repository.TryFindUser(login);
        switch (userFound)
        {
            case false:
                return Identity.Fail(AuthenticationError.WrongLogin);
            case true when !user.Activated:
                return Identity.Fail(AuthenticationError.Inactive);
            case true when user.IsRemoved:
                return Identity.Fail(AuthenticationError.Removed);
            case true when user.AccessPolicy.HasFlag(AccessPolicy.FullBan):
                return Identity.Fail(AuthenticationError.Banned);
            case true when !securityManager.ComparePasswords(password, user.Salt, user.PasswordHash):
                // todo: brute force protection
                return Identity.Fail(AuthenticationError.WrongPassword);

            default:
                var session = sessionFactory.Create(persistent, false);
                var settings = await repository.FindUserSettings(user.UserId);
                return await CreateAuthenticationResult(user, session, settings);
        }
    }

    /// <inheritdoc />
    public async Task<IIdentity> Authenticate(string authToken)
    {
        Guid userId;
        Guid sessionId;

        try
        {
            var decryptedString = await cryptoService.Decrypt(authToken);
            var authData = JsonSerializer.Deserialize<Dictionary<string, Guid>>(decryptedString);
            userId = authData[UserIdKey];
            sessionId = authData[SessionIdKey];
        }
        catch
        {
            return Identity.Fail(AuthenticationError.ForgedToken);
        }

        var fetchUser = repository.FindUser(userId);
        var fetchSession = repository.FindUserSession(sessionId);
        var fetchSettings = repository.FindUserSettings(userId);

        await Task.WhenAll(fetchUser, fetchSession, fetchSettings);

        var user = await fetchUser;
        var session = await fetchSession;
        var settings = await fetchSettings;

        if (session == null)
        {
            return Identity.Fail(AuthenticationError.SessionExpired);
        }

        if (!session.Persistent &&
            session.ExpirationDate < dateTimeProvider.Now)
        {
            await repository.RemoveSession(userId, sessionId);
            return Identity.Fail(AuthenticationError.SessionExpired);
        }

        var sessionRefreshDelta = TimeSpan.FromMinutes(20);
        if (!session.Persistent &&
            session.ExpirationDate < dateTimeProvider.Now + sessionRefreshDelta)
        {
            await repository.RefreshSession(userId, sessionId, session.ExpirationDate + sessionRefreshDelta);
        }

        if (!session.Invisible && (
                !user.LastVisitDate.HasValue ||
                dateTimeProvider.Now - user.LastVisitDate.Value > TimeSpan.FromMinutes(1)))
        {
            var userUpdate = updateBuilderFactory.Create<User>(user.UserId)
                .Field(u => u.LastVisitDate, dateTimeProvider.Now);
            await repository.UpdateActivity(userUpdate);
        }

        return Identity.Success(user, session, settings, authToken);
    }

    /// <inheritdoc />
    public async Task<IIdentity> Authenticate(Guid userId)
    {
        var user = await repository.FindUser(userId);
        var session = sessionFactory.Create(false, true);
        var settings = await repository.FindUserSettings(userId);
        return await CreateAuthenticationResult(user, session, settings);
    }

    /// <inheritdoc />
    public async Task<IIdentity> Logout()
    {
        var identity = identityProvider.Current;
        await repository.RemoveSession(identity.User.UserId, identity.Session.Id);
        return Identity.Guest();
    }

    /// <inheritdoc />
    public async Task<IIdentity> LogoutElsewhere()
    {
        var identity = identityProvider.Current;
        await repository.RemoveSessionsExcept(identity.User.UserId, identity.Session.Id);
        return identity;
    }

    private async Task<IIdentity> CreateAuthenticationResult(
        AuthenticatedUser user, DbSession session, UserSettings settings)
    {
        var newSession = await repository.AddSession(user.UserId, session);
        var authData = new Dictionary<string, Guid>
        {
            [UserIdKey] = user.UserId,
            [SessionIdKey] = session.Id
        };
        var token = await cryptoService.Encrypt(JsonSerializer.Serialize(authData));
        return Identity.Success(user, newSession, settings, token);
    }
}
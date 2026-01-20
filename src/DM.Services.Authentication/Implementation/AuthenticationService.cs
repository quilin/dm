using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
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
internal class AuthenticationService(
    ISecurityManager securityManager,
    ISymmetricCryptoService cryptoService,
    IAuthenticationRepository repository,
    ISessionFactory sessionFactory,
    IDateTimeProvider dateTimeProvider,
    IIdentityProvider identityProvider,
    IUpdateBuilderFactory updateBuilderFactory) : IAuthenticationService
{
    private const string UserIdKey = "userId";
    private const string SessionIdKey = "sessionId";

    /// <inheritdoc />
    public async Task<IIdentity> Authenticate(
        string login, string password, bool persistent, CancellationToken cancellationToken)
    {
        var (userFound, user) = await repository.TryFindUser(login, cancellationToken);
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
                var settings = await repository.FindUserSettings(user.UserId, cancellationToken);
                return await CreateAuthenticationResult(user, session, settings, cancellationToken);
        }
    }

    /// <inheritdoc />
    public async Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
    {
        Guid userId;
        Guid sessionId;

        try
        {
            var decryptedString = await cryptoService.Decrypt(authToken, cancellationToken);
            var authData = JsonSerializer.Deserialize<Dictionary<string, Guid>>(decryptedString);
            userId = authData[UserIdKey];
            sessionId = authData[SessionIdKey];
        }
        catch
        {
            return Identity.Fail(AuthenticationError.ForgedToken);
        }

        var fetchUser = repository.FindUser(userId, cancellationToken);
        var fetchSession = repository.FindUserSession(sessionId, cancellationToken);
        var fetchSettings = repository.FindUserSettings(userId, cancellationToken);

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
            await repository.RemoveSession(userId, sessionId, cancellationToken);
            return Identity.Fail(AuthenticationError.SessionExpired);
        }

        var sessionRefreshDelta = TimeSpan.FromMinutes(20);
        if (!session.Persistent &&
            session.ExpirationDate < dateTimeProvider.Now + sessionRefreshDelta)
        {
            await repository.RefreshSession(
                userId, sessionId, session.ExpirationDate + sessionRefreshDelta, cancellationToken);
        }

        if (!session.Invisible && (
                !user.LastVisitDate.HasValue ||
                dateTimeProvider.Now - user.LastVisitDate.Value > TimeSpan.FromMinutes(1)))
        {
            var userUpdate = updateBuilderFactory.Create<User>(user.UserId)
                .Field(u => u.LastVisitDate, dateTimeProvider.Now);
            await repository.UpdateActivity(userUpdate, cancellationToken);
        }

        return Identity.Success(user, session, settings, authToken);
    }

    /// <inheritdoc />
    public async Task<IIdentity> Authenticate(Guid userId, CancellationToken cancellationToken)
    {
        var user = await repository.FindUser(userId, cancellationToken);
        var session = sessionFactory.Create(false, true);
        var settings = await repository.FindUserSettings(userId, cancellationToken);
        return await CreateAuthenticationResult(user, session, settings, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IIdentity> Logout(CancellationToken cancellationToken)
    {
        var identity = identityProvider.Current;
        await repository.RemoveSession(identity.User.UserId, identity.Session.Id, cancellationToken);
        return Identity.Guest();
    }

    /// <inheritdoc />
    public async Task<IIdentity> LogoutElsewhere(CancellationToken cancellationToken)
    {
        var identity = identityProvider.Current;
        await repository.RemoveSessionsExcept(identity.User.UserId, identity.Session.Id, cancellationToken);
        return identity;
    }

    private async Task<IIdentity> CreateAuthenticationResult(
        AuthenticatedUser user, DbSession session, UserSettings settings, CancellationToken cancellationToken)
    {
        var newSession = await repository.AddSession(user.UserId, session, cancellationToken);
        var authData = new Dictionary<string, Guid>
        {
            [UserIdKey] = user.UserId,
            [SessionIdKey] = session.Id
        };
        var token = await cryptoService.Encrypt(JsonSerializer.Serialize(authData), cancellationToken);
        return Identity.Success(user, newSession, settings, token);
    }
}
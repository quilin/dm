using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using DbUserSettings = DM.Services.DataAccess.BusinessObjects.Users.Settings.UserSettings;
using DbSession = DM.Services.DataAccess.BusinessObjects.Users.Session;
using Session = DM.Services.Authentication.Dto.Session;

namespace DM.Services.Authentication.Repositories;

/// <inheritdoc cref="IAuthenticationRepository" />
internal class AuthenticationRepository : MongoRepository, IAuthenticationRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public AuthenticationRepository(
        DmDbContext dbContext,
        DmMongoClient mongoClient,
        IMapper mapper) : base(mongoClient)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<(bool Success, AuthenticatedUser User)> TryFindUser(string login)
    {
        var result = await dbContext.Users
            .Where(u => u.Login.ToLower() == login.ToLower())
            .ProjectTo<AuthenticatedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        return (result != null, result);
    }

    /// <inheritdoc />
    public Task<AuthenticatedUser> FindUser(Guid userId)
    {
        return dbContext.Users
            .Where(u => u.UserId == userId)
            .ProjectTo<AuthenticatedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<Session> FindUserSession(Guid sessionId)
    {
        var userSessions = await Collection<UserSessions>()
            .Find(Filter<UserSessions>()
                .ElemMatch(u => u.Sessions, s => s.Id == sessionId))
            .FirstOrDefaultAsync();
        var matchingSession = userSessions?.Sessions.FirstOrDefault(s => s.Id == sessionId);
        return matchingSession == null
            ? null
            : mapper.Map<Session>(matchingSession);
    }

    /// <inheritdoc />
    public async Task<UserSettings> FindUserSettings(Guid userId)
    {
        var settings = await Collection<DbUserSettings>()
            .Find(Filter<DbUserSettings>()
                .Eq(u => u.UserId, userId))
            .Project(Project<DbUserSettings>()
                .Expression(s => new UserSettings
                {
                    Id = s.UserId,
                    ColorSchema = s.ColorSchema,
                    Paging = new PagingSettings
                    {
                        PostsPerPage = s.Paging.PostsPerPage,
                        CommentsPerPage = s.Paging.CommentsPerPage,
                        MessagesPerPage = s.Paging.MessagesPerPage,
                        TopicsPerPage = s.Paging.TopicsPerPage,
                        EntitiesPerPage = s.Paging.EntitiesPerPage
                    },
                    NannyGreetingsMessage = s.NannyGreetingsMessage
                }))
            .FirstOrDefaultAsync();
        return settings ?? UserSettings.Default;
    }

    /// <inheritdoc />
    public Task RemoveSession(Guid userId, Guid sessionId)
    {
        return Collection<UserSessions>().FindOneAndUpdateAsync(
            Filter<UserSessions>().Eq(u => u.Id, userId),
            Update<UserSessions>().PullFilter(s => s.Sessions, s => s.Id == sessionId));
    }

    /// <inheritdoc />
    public Task RefreshSession(Guid userId, Guid sessionId, DateTimeOffset expirationDate)
    {
        return Collection<UserSessions>().FindOneAndUpdateAsync(
            Filter<UserSessions>().Eq(u => u.Id, userId) &
            Filter<UserSessions>().ElemMatch(u => u.Sessions, s => s.Id == sessionId),
            Update<UserSessions>().Set(u => u.Sessions[-1].ExpirationDate, expirationDate.UtcDateTime));
    }

    /// <inheritdoc />
    public async Task<Session> AddSession(Guid userId, DbSession session)
    {
        await Collection<UserSessions>().FindOneAndUpdateAsync(
            Filter<UserSessions>().Eq(u => u.Id, userId),
            Update<UserSessions>().Push(s => s.Sessions, session),
            new FindOneAndUpdateOptions<UserSessions> {IsUpsert = true});
        return mapper.Map<Session>(session);
    }

    /// <inheritdoc />
    public Task RemoveSessionsExcept(Guid userId, Guid sessionId)
    {
        return Collection<UserSessions>().FindOneAndUpdateAsync(
            Filter<UserSessions>().Eq(u => u.Id, userId),
            Update<UserSessions>().PullFilter(s => s.Sessions, s => s.Id != sessionId),
            new FindOneAndUpdateOptions<UserSessions>{IsUpsert =  true});
    }

    /// <inheritdoc />
    public Task UpdateActivity(IUpdateBuilder<User> userUpdate)
    {
        userUpdate.AttachTo(dbContext);
        return dbContext.SaveChangesAsync();
    }
}
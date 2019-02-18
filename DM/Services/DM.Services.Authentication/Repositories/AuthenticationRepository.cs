using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using DbUserSettings = DM.Services.DataAccess.BusinessObjects.Users.Settings.UserSettings;

namespace DM.Services.Authentication.Repositories
{
    internal class AuthenticationRepository : MongoRepository, IAuthenticationRepository
    {
        private readonly ReadDmDbContext dbContext;

        public AuthenticationRepository(
            ReadDmDbContext dbContext,
            DmMongoClient mongoClient) : base(mongoClient)
        {
            this.dbContext = dbContext;
        }

        private static readonly Expression<Func<User, AuthenticatedUser>> MapAuthenticatedUser = user =>
            new AuthenticatedUser
            {
                UserId = user.UserId,
                Login = user.Login,
                ProfilePictureUrl = user.ProfilePictures
                    .Where(u => !u.IsRemoved)
                    .Select(u => u.VirtualPath)
                    .FirstOrDefault(),
                Role = user.Role,
                LastVisitDate = user.LastVisitDate,
                RatingDisabled = user.RatingDisabled,
                QualityRating = user.QualityRating,
                QuantityRating = user.QuantityRating,
                Activated = user.Activated,
                Salt = user.Salt,
                PasswordHash = user.PasswordHash,
                IsRemoved = user.IsRemoved,
                AccessPolicy = user.AccessPolicy,
                AccessRestrictionPolicies = user.BansReceived
                    .Where(b => !b.IsRemoved)
                    .Select(b => b.AccessRestrictionPolicy)
                    .ToArray()
            };

        public async Task<(bool Success, AuthenticatedUser User)> TryFindUser(string login)
        {
            var result = await dbContext.Users
                .Where(u => u.Login.ToLower() == login.ToLower())
                .Select(MapAuthenticatedUser)
                .FirstOrDefaultAsync();
            return (result != null, result);
        }

        public Task<AuthenticatedUser> FindUser(Guid userId)
        {
            return dbContext.Users
                .Where(u => u.UserId == userId)
                .Select(MapAuthenticatedUser)
                .FirstAsync();
        }

        public async Task<Session> FindUserSession(Guid sessionId)
        {
            var userSessions = await Collection<UserSessions>()
                .Find(Filter<UserSessions>().ElemMatch(u => u.Sessions, s => s.Id == sessionId))
                .FirstAsync();
            return userSessions.Sessions.First();
        }

        public async Task<UserSettings> FindUserSettings(Guid userId)
        {
            return await Collection<DbUserSettings>()
                .Find(Filter<DbUserSettings>().Eq(u => u.Id, userId))
                .Project(Select<DbUserSettings>().Expression(s => new UserSettings
                {
                    Id = s.Id,
                    ColorScheme = (ColorScheme) s.ColorScheme,
                    PostsPerPage = s.Paging.PostsPerPage,
                    CommentsPerPage = s.Paging.CommentsPerPage,
                    MessagesPerPage = s.Paging.MessagesPerPage,
                    TopicsPerPage = s.Paging.TopicsPerPage,
                    NannyGreetingsMessage = s.NannyGreetingsMessage
                }))
                .FirstOrDefaultAsync() ?? UserSettings.Default;
        }

        public Task RemoveSession(Guid userId, Guid sessionId)
        {
            return Collection<UserSessions>().FindOneAndUpdateAsync(
                Filter<UserSessions>().Eq(u => u.Id, userId),
                Update<UserSessions>().PullFilter(s => s.Sessions, s => s.Id == sessionId));
        }

        public Task RefreshSession(Guid userId, Guid sessionId, DateTime expirationDate)
        {
            return Collection<UserSessions>().FindOneAndUpdateAsync(
                Filter<UserSessions>().Eq(u => u.Id, userId) &
                Filter<UserSessions>().ElemMatch(u => u.Sessions, s => s.Id == sessionId),
                Update<UserSessions>().Set(u => u.Sessions[-1].ExpirationDate, expirationDate));
        }

        public Task AddSession(Guid userId, Session session)
        {
            return Collection<UserSessions>().FindOneAndUpdateAsync(
                Filter<UserSessions>().Eq(u => u.Id, userId),
                Update<UserSessions>().Push(s => s.Sessions, session),
                new FindOneAndUpdateOptions<UserSessions> {IsUpsert = true});
        }

        public Task RemoveSessions(Guid userId)
        {
            return Collection<UserSessions>().FindOneAndUpdateAsync(
                Filter<UserSessions>().Eq(u => u.Id, userId),
                Update<UserSessions>().Set(s => s.Sessions, new List<Session>()),
                new FindOneAndUpdateOptions<UserSessions> {IsUpsert = true});
        }
    }
}
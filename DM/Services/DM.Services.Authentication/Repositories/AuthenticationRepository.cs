using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DM.Services.Authentication.Repositories
{
    public class AuthenticationRepository : MongoRepository<UserSessions>, IAuthenticationRepository
    {
        private readonly DmDbContext dbContext;

        public AuthenticationRepository(
            DmDbContext dbContext,
            DmMongoClient mongoClient) : base(mongoClient)
        {
            this.dbContext = dbContext;
        }

        private static readonly Func<User, AuthenticatedUser> MapAuthenticatedUser = user => new AuthenticatedUser
        {
            UserId = user.UserId,
            Login = user.Login,
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
            AccessRestrictionPolicies = user.BansReceived.Select(b => b.AccessRestrictionPolicy).ToArray()
        };

        public async Task<(bool Success, AuthenticatedUser User)> TryFindUser(string login)
        {
            var result = await dbContext.Users.AsNoTracking()
                .Include(u => u.BansReceived)
                .Where(u => u.Login == login)
                .Select(u => MapAuthenticatedUser.Invoke(u))
                .FirstOrDefaultAsync();
            return (result != null, result);
        }

        public Task<AuthenticatedUser> FindUser(Guid userId)
        {
            return dbContext.Users.AsNoTracking()
                .Include(u => u.BansReceived)
                .Where(u => u.UserId == userId)
                .Select(u => MapAuthenticatedUser.Invoke(u))
                .FirstAsync();
        }

        public async Task<Session> FindUserSession(Guid sessionId)
        {
            var userSessions = await Collection
                .Find(Filter.ElemMatch(u => u.Sessions, s => s.Id == sessionId))
                .FirstAsync();
            return userSessions.Sessions.First();
        }

        public Task RemoveSession(Guid userId, Guid sessionId)
        {
            return Collection.FindOneAndUpdateAsync(
                Filter.Eq(u => u.Id, userId),
                Update.PullFilter(s => s.Sessions, s => s.Id == sessionId));
        }

        public Task RefreshSession(Guid userId, Guid sessionId, DateTime expirationDate)
        {
            return Collection.FindOneAndUpdateAsync(
                Filter.Eq(u => u.Id, userId) &
                Filter.ElemMatch(u => u.Sessions, s => s.Id == sessionId),
                Update.Set(u => u.Sessions[-1].ExpirationDate, expirationDate));
        }

        public Task AddSession(Guid userId, Session session)
        {
            return Collection.FindOneAndUpdateAsync(
                Filter.Eq(u => u.Id, userId),
                Update.Push(s => s.Sessions, session),
                new FindOneAndUpdateOptions<UserSessions> {IsUpsert = true});
        }

        public async Task<IEnumerable<IntentionBan>> GetActiveUserBans(Guid userId)
        {
            return await dbContext.Bans.AsNoTracking()
                .Where(b => b.StartDate <= DateTime.UtcNow &&
                            b.EndDate >= DateTime.UtcNow &&
                            !b.IsRemoved &&
                            b.UserId == userId)
                .Select(b => new IntentionBan
                {
                    AccessPolicy = b.AccessRestrictionPolicy
                })
                .ToArrayAsync();
        }
    }
}
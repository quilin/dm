using System;
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
        
        public async Task<(bool Success, AuthenticatingUser User)> TryFindUser(string login)
        {
            var result = await dbContext.Users.AsNoTracking()
                .Where(u => u.Login == login)
                .Select(AuthenticatingUser.FromDal)
                .FirstOrDefaultAsync();
            return (result != null, result);
        }

        public Task<AuthenticatingUser> FindUser(Guid userId)
        {
            return dbContext.Users.AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(AuthenticatingUser.FromDal)
                .FirstAsync();
        }

        public async Task<Session> FindUserSession(Guid sessionId)
        {
            var userSessions = await Collection
                .Find(Filter.ElemMatch(s => s.Sessions, s => s.Id == sessionId))
                .FirstAsync();
            return userSessions.Sessions.First(s => s.Id == sessionId);
        }
    }
}
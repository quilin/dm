using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Authentication.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DmDbContext dbContext;

        public AuthenticationRepository(
            DmDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public Task<AuthenticatingUser> Find(string login)
        {
            return dbContext.Users.AsNoTracking()
                .Where(u => u.Login == login)
                .Select(u => AuthenticatingUser.FromDal.Invoke(u))
                .FirstOrDefaultAsync();
        }

        public Task<AuthenticatingUser> Find(Guid userId)
        {
            return dbContext.Users.AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => AuthenticatingUser.FromDal.Invoke(u))
                .FirstAsync();
        }
    }
}
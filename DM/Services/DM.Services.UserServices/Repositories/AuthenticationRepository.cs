using System.Linq;
using System.Threading.Tasks;
using DM.Services.DataAccess;
using DM.Services.UserServices.Dto;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.UserServices.Repositories
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
                .Select(u => new AuthenticatingUser
                {
                    Id = u.UserId,
                    Activated = u.Activated,
                    Salt = u.Salt,
                    PasswordHash = u.PasswordHash
                })
                .FirstOrDefaultAsync();
        }
    }
}
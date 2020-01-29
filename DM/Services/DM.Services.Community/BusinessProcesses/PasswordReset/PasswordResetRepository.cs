using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.PasswordReset
{
    /// <inheritdoc />
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public PasswordResetRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public Task<GeneralUser> FindUser(string login) => dbContext.Users
            .Where(u => u.Login.ToLower() == login.ToLower())
            .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        /// <inheritdoc />
        public Task CreateToken(Token token)
        {
            dbContext.Tokens.Add(token);
            return dbContext.SaveChangesAsync();
        }
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Authentication.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.BusinessProcesses.Account.EmailChange;

/// <inheritdoc />
internal class EmailChangeRepository(
    DmDbContext dbContext,
    IMapper mapper) : IEmailChangeRepository
{
    /// <inheritdoc />
    public Task<AuthenticatedUser> FindUser(string login, CancellationToken cancellationToken) => dbContext.Users
        .Where(u => u.Login.ToLower() == login.ToLower())
        .ProjectTo<AuthenticatedUser>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public Task Update(IUpdateBuilder<User> updateUser, Token token, CancellationToken cancellationToken)
    {
        updateUser.AttachTo(dbContext);
        dbContext.Tokens.Add(token);
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
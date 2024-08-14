using System.Linq;
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
internal class EmailChangeRepository : IEmailChangeRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public EmailChangeRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public Task<AuthenticatedUser> FindUser(string login) => dbContext.Users
        .Where(u => u.Login.ToLower() == login.ToLower())
        .ProjectTo<AuthenticatedUser>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();

    /// <inheritdoc />
    public Task Update(IUpdateBuilder<User> updateUser, Token token)
    {
        updateUser.AttachTo(dbContext);
        dbContext.Tokens.Add(token);
        return dbContext.SaveChangesAsync();
    }
}
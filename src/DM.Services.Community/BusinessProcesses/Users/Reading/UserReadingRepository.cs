using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DM.Services.Community.BusinessProcesses.Users.Reading;

/// <inheritdoc />
internal class UserReadingRepository : MongoCollectionRepository<UserSettings>, IUserReadingRepository
{
    private readonly DmDbContext dmDbContext;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public UserReadingRepository(
        DmDbContext dmDbContext,
        DmMongoClient mongoClient,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper) : base(mongoClient)
    {
        this.dmDbContext = dmDbContext;
        this.dateTimeProvider = dateTimeProvider;
        this.mapper = mapper;
    }

    private static readonly TimeSpan ActivityRange = TimeSpan.FromDays(30);

    /// <inheritdoc />
    public Task<int> CountUsers(bool withInactive) => GetQuery(withInactive).CountAsync();

    /// <inheritdoc />
    public async Task<IEnumerable<GeneralUser>> GetUsers(PagingData paging, bool withInactive) =>
        await GetQuery(withInactive)
            .OrderBy(u => u.RatingDisabled)
            .ThenByDescending(u => u.QualityRating)
            .ThenBy(u => u.QuantityRating)
            .Page(paging)
            .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
            .ToArrayAsync();

    private IQueryable<User> GetQuery(bool withInactive)
    {
        var query = dmDbContext.Users.Where(u => !u.IsRemoved && u.Activated);
        if (withInactive)
        {
            return query;
        }

        var activeRange = dateTimeProvider.Now - ActivityRange;
        return query.Where(u =>
            u.LastVisitDate.HasValue &&
            u.LastVisitDate > activeRange);
    }

    /// <inheritdoc />
    public Task<GeneralUser> GetUser(string login) => dmDbContext.Users
        .Where(u => !u.IsRemoved && u.Activated && u.Login.ToLower() == login.ToLower())
        .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();

    /// <inheritdoc />
    public async Task<UserDetails> GetUserDetails(string login)
    {
        var userDetails = await dmDbContext.Users
            .Where(u => !u.IsRemoved && u.Activated && u.Login.ToLower() == login.ToLower())
            .ProjectTo<UserDetails>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (userDetails == null)
        {
            return null;
        }

        var userSettings = await Collection
            .Find(Filter.Eq(u => u.UserId, userDetails.UserId))
            .FirstOrDefaultAsync();
        userDetails.Settings = userSettings == null
            ? Authentication.Dto.UserSettings.Default
            : mapper.Map<Authentication.Dto.UserSettings>(userSettings);
        return userDetails;
    }
}
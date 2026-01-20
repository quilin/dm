using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DM.Services.Community.Storage.Storages.Users;

/// <inheritdoc />
internal class UserReadingRepository(
    DmDbContext dmDbContext,
    DmMongoClient mongoClient,
    IDateTimeProvider dateTimeProvider,
    IMapper mapper) : MongoCollectionRepository<UserSettings>(mongoClient), IUserReadingRepository
{
    private static readonly TimeSpan ActivityRange = TimeSpan.FromDays(30);

    /// <inheritdoc />
    public Task<int> CountUsers(bool withInactive, CancellationToken cancellationToken) =>
        GetQuery(withInactive).CountAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<GeneralUser>> GetUsers(PagingData paging, bool withInactive,
        CancellationToken cancellationToken) =>
        await GetQuery(withInactive)
            .OrderBy(u => u.RatingDisabled)
            .ThenByDescending(u => u.QualityRating)
            .ThenBy(u => u.QuantityRating)
            .Page(paging)
            .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
            .ToArrayAsync(cancellationToken);

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
    public Task<GeneralUser?> GetUser(string login, CancellationToken cancellationToken) =>
        dmDbContext.Users
            .Where(u => !u.IsRemoved && u.Activated && u.Login.ToLower() == login.ToLower())
            .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<UserDetails?> GetUserDetails(string login, CancellationToken cancellationToken)
    {
        var userDetails = await dmDbContext.Users
            .Where(u => !u.IsRemoved && u.Activated && u.Login.ToLower() == login.ToLower())
            .ProjectTo<UserDetails>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (userDetails == null)
        {
            return null;
        }

        var userSettings = await Collection
            .Find(Filter.Eq(u => u.UserId, userDetails.UserId))
            .FirstOrDefaultAsync(cancellationToken);
        userDetails.Settings = userSettings == null
            ? Authentication.Dto.UserSettings.Default
            : mapper.Map<Authentication.Dto.UserSettings>(userSettings);
        return userDetails;
    }
}
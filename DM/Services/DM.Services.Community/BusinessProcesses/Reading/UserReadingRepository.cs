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
using Microsoft.EntityFrameworkCore;
using UserProfile = DM.Services.Community.Dto.UserProfile;

namespace DM.Services.Community.BusinessProcesses.Reading
{
    /// <inheritdoc />
    public class UserReadingRepository : IUserReadingRepository
    {
        private readonly DmDbContext dmDbContext;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public UserReadingRepository(
            DmDbContext dmDbContext,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper)
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
        public Task<GeneralUser> GetUser(string login)
        {
            var lowerLogin = login.ToLower();
            return dmDbContext.Users
                .Where(u => !u.IsRemoved && u.Activated && u.Login.ToLower() == lowerLogin)
                .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public Task<UserProfile> GetProfile(string login)
        {
            var lowerLogin = login.ToLower();
            return dmDbContext.Users
                .Where(u => !u.IsRemoved && u.Activated && u.Login.ToLower() == lowerLogin)
                .ProjectTo<UserProfile>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
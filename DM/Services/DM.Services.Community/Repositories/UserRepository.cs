using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Community.Repositories
{
    /// <inheritdoc />
    public class UserRepository : IUserRepository
    {
        private readonly DmDbContext dmDbContext;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public UserRepository(
            DmDbContext dmDbContext,
            IDateTimeProvider dateTimeProvider)
        {
            this.dmDbContext = dmDbContext;
            this.dateTimeProvider = dateTimeProvider;
        }

        private static readonly TimeSpan ActivityRange = TimeSpan.FromDays(30);

        /// <inheritdoc />
        public Task<int> CountUsers(bool withInactive)
        {
            var query = dmDbContext.Users.Where(u => !u.IsRemoved);
            if (withInactive)
            {
                return query.CountAsync();
            }

            var activeRange = dateTimeProvider.Now - ActivityRange;
            return query
                .Where(u => u.LastVisitDate > activeRange)
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GeneralUser>> GetUsers(PagingData paging, bool withInactive)
        {
            var query = dmDbContext.Users.Where(u => !u.IsRemoved);
            if (!withInactive)
            {
                var activeRange = dateTimeProvider.Now - ActivityRange;
                query = query.Where(u => u.LastVisitDate > activeRange);
            }

            return await query
                .OrderBy(u => u.RatingDisabled)
                .ThenByDescending(u => u.QualityRating)
                .ThenBy(u => u.QuantityRating)
                .Page(paging)
                .ProjectTo<GeneralUser>()
                .ToArrayAsync();
        }

        /// <inheritdoc />
        public Task<GeneralUser> GetUser(string login)
        {
            var lowerLogin = login.ToLower();
            return dmDbContext.Users
                .Where(u => !u.IsRemoved && u.Login.ToLower() == lowerLogin)
                .ProjectTo<GeneralUser>()
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public Task<UserProfile> GetProfile(string login)
        {
            var lowerLogin = login.ToLower();
            return dmDbContext.Users
                .Where(u => !u.IsRemoved && u.Login.ToLower() == lowerLogin)
                .ProjectTo<UserProfile>()
                .FirstOrDefaultAsync();
        }
    }
}
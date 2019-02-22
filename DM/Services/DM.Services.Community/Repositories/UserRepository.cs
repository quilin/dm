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
    public class UserRepository : IUserRepository
    {
        private readonly ReadDmDbContext dmDbContext;
        private readonly IDateTimeProvider dateTimeProvider;

        public UserRepository(
            ReadDmDbContext dmDbContext,
            IDateTimeProvider dateTimeProvider)
        {
            this.dmDbContext = dmDbContext;
            this.dateTimeProvider = dateTimeProvider;
        }

        private static readonly TimeSpan ActivityRange = TimeSpan.FromDays(30);
        
        public Task<int> CountUsers(bool withInactive)
        {
            var query = dmDbContext.Users.Where(u => !u.IsRemoved);
            if (!withInactive)
            {
                var activeRange = dateTimeProvider.Now - ActivityRange;
                query = query.Where(u => u.LastVisitDate > activeRange);
            }

            return query.CountAsync();
        }

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

        public Task<GeneralUser> GetUser(string login)
        {
            var lowerLogin = login.ToLower();
            return dmDbContext.Users
                .Where(u => !u.IsRemoved && u.Login.ToLower() == lowerLogin)
                .ProjectTo<GeneralUser>()
                .FirstOrDefaultAsync();
        }

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
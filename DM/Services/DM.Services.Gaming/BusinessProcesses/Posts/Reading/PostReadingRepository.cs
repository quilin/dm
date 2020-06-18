using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess;
using DM.Services.Gaming.BusinessProcesses.Shared;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Posts.Reading
{
    /// <inheritdoc />
    public class PostReadingRepository : IPostReadingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public PostReadingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        
        /// <inheritdoc />
        public Task<int> Count(Guid roomId, Guid userId)
        {
            return dbContext.Posts
                .Where(p => !p.IsRemoved && p.RoomId == roomId)
                .Where(p => AccessibilityFilters.RoomAvailable(userId).Compile().Invoke(p.Room))
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Post>> Get(Guid roomId, PagingData paging, Guid userId)
        {
            return await dbContext.Posts
                .Where(p => !p.IsRemoved && p.RoomId == roomId)
                .Where(p => AccessibilityFilters.RoomAvailable(userId).Compile().Invoke(p.Room))
                .OrderBy(p => p.CreateDate)
                .Page(paging)
                .ProjectTo<Post>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }

        /// <inheritdoc />
        public Task<Post> Get(Guid postId, Guid userId)
        {
            return dbContext.Posts
                .Where(p => !p.IsRemoved && p.PostId == postId)
                .Where(p => AccessibilityFilters.RoomAvailable(userId).Compile().Invoke(p.Room))
                .ProjectTo<Post>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
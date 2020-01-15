using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Reading
{
    /// <inheritdoc cref="ISchemaReadingRepository" />
    public class SchemaReadingRepository :
        MongoCollectionRepository<AttributeSchema>,
        ISchemaReadingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public SchemaReadingRepository(
            DmDbContext dbContext,
            DmMongoClient client,
            IMapper mapper) : base(client)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AttributeSchema>> GetSchemata(Guid userId)
        {
            return await Collection
                .Find(Filter.Eq(s => s.Type, SchemaType.Public) | Filter.Eq(s => s.UserId, userId))
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<AttributeSchema> GetSchema(Guid schemaId)
        {
            return await Collection
                .Find(Filter.Eq(s => s.Id, schemaId))
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GeneralUser>> GetSchemataAuthors(ICollection<Guid> userIds)
        {
            return await dbContext.Users
                .Where(u => userIds.Contains(u.UserId))
                .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}
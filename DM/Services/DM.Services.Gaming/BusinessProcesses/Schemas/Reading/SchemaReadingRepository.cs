using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Reading
{
    /// <inheritdoc cref="ISchemaReadingRepository" />
    public class SchemaReadingRepository :
        MongoCollectionRepository<AttributeSchema>,
        ISchemaReadingRepository
    {
        /// <inheritdoc />
        public SchemaReadingRepository(DmMongoClient client) : base(client)
        {
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.Gaming.Dto.Output;
using MongoDB.Driver;
using DbSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Reading
{
    /// <inheritdoc cref="ISchemaReadingRepository" />
    public class SchemaReadingRepository :
        MongoCollectionRepository<DbSchema>,
        ISchemaReadingRepository
    {
        /// <inheritdoc />
        public SchemaReadingRepository(DmMongoClient client) : base(client)
        {
        }

        /// <summary>
        /// Basic projection for schema
        /// </summary>
        public static readonly Expression<Func<DbSchema, AttributeSchema>> SchemaProjection = s => new AttributeSchema
        {
            Id = s.Id,
            Title = s.Name,
            UserId = s.UserId,
            Type = s.Type,
            Specifications = s.Specifications
        };

        /// <inheritdoc />
        public async Task<IEnumerable<AttributeSchema>> GetSchemata(Guid userId)
        {
            return await Collection
                .Find(Filter.Eq(s => s.Type, SchemaType.Public) | Filter.Eq(s => s.UserId, userId))
                .Project(Project.Expression(SchemaProjection))
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<AttributeSchema> GetSchema(Guid schemaId)
        {
            return await Collection
                .Find(Filter.Eq(s => s.Id, schemaId))
                .Project(Project.Expression(SchemaProjection))
                .FirstOrDefaultAsync();
        }
    }
}
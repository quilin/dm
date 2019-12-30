using System.Threading.Tasks;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Output;
using MongoDB.Driver;
using DbAttributeSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating
{
    /// <inheritdoc />
    public class SchemaCreatingRepository : MongoCollectionRepository<DbAttributeSchema>, ISchemaCreatingRepository
    {
        /// <inheritdoc />
        public SchemaCreatingRepository(DmMongoClient client) : base(client)
        {
        }

        /// <inheritdoc />
        public async Task<AttributeSchema> Create(DbAttributeSchema schema)
        {
            await Collection.InsertOneAsync(schema);
            return await Collection
                .Find(Filter.Eq(s => s.Id, schema.Id))
                .Project(SchemaReadingRepository.SchemaProjection)
                .FirstAsync();
        }
    }
}
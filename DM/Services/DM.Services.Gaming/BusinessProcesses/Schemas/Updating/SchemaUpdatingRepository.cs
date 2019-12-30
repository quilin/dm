using System.Threading.Tasks;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Output;
using MongoDB.Driver;
using DbAttributeSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Updating
{
    /// <inheritdoc />
    public class SchemaUpdatingRepository : MongoCollectionRepository<DbAttributeSchema>, ISchemaUpdatingRepository
    {
        /// <inheritdoc />
        public SchemaUpdatingRepository(DmMongoClient client) : base(client)
        {
        }

        /// <inheritdoc />
        public async Task<AttributeSchema> Update(DbAttributeSchema schema)
        {
            await Collection.ReplaceOneAsync(Filter.Eq(s => s.Id, schema.Id), schema);
            return await Collection
                .Find(Filter.Eq(s => s.Id, schema.Id))
                .Project(SchemaReadingRepository.SchemaProjection)
                .FirstAsync();
        }
    }
}
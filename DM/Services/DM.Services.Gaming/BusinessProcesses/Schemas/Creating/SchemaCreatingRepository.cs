using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating
{
    /// <inheritdoc />
    public class SchemaCreatingRepository : MongoCollectionRepository<AttributeSchema>, ISchemaCreatingRepository
    {
        /// <inheritdoc />
        public SchemaCreatingRepository(DmMongoClient client) : base(client)
        {
        }

        /// <inheritdoc />
        public async Task<AttributeSchema> Create(AttributeSchema schema)
        {
            await Collection.InsertOneAsync(schema);
            return await Collection
                .Find(Filter.Eq(s => s.Id, schema.Id))
                .FirstAsync();
        }
    }
}
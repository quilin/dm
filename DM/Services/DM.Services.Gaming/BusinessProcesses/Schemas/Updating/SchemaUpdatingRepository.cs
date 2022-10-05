using System.Threading.Tasks;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;
using DbAttributeSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Updating;

/// <inheritdoc />
internal class SchemaUpdatingRepository : MongoCollectionRepository<DbAttributeSchema>, ISchemaUpdatingRepository
{
    /// <inheritdoc />
    public SchemaUpdatingRepository(DmMongoClient client) : base(client)
    {
    }

    /// <inheritdoc />
    public async Task<DbAttributeSchema> UpdateSchema(DbAttributeSchema schema)
    {
        await Collection.ReplaceOneAsync(Filter.Eq(s => s.Id, schema.Id), schema);
        return await Collection
            .Find(Filter.Eq(s => s.Id, schema.Id))
            .FirstAsync();
    }
}
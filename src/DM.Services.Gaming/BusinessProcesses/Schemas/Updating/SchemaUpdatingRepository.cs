using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess.MongoIntegration;
using MongoDB.Driver;
using DbAttributeSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Updating;

/// <inheritdoc />
internal class SchemaUpdatingRepository(DmMongoClient client)
    : MongoCollectionRepository<DbAttributeSchema>(client), ISchemaUpdatingRepository
{
    /// <inheritdoc />
    public async Task<DbAttributeSchema> UpdateSchema(DbAttributeSchema schema, CancellationToken cancellationToken)
    {
        await Collection.ReplaceOneAsync(Filter.Eq(s => s.Id, schema.Id), schema,
            cancellationToken: cancellationToken);
        return await Collection
            .Find(Filter.Eq(s => s.Id, schema.Id))
            .FirstAsync(cancellationToken);
    }
}
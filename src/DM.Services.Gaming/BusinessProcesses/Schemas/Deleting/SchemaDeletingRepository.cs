using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Deleting;

/// <inheritdoc />
internal class SchemaDeletingRepository(DmMongoClient client)
    : MongoCollectionRepository<AttributeSchema>(client), ISchemaDeletingRepository
{
    /// <inheritdoc />
    public async Task Delete(Guid schemaId, CancellationToken cancellationToken) =>
        await Collection.DeleteOneAsync(Filter.Eq(s => s.Id, schemaId), cancellationToken);
}
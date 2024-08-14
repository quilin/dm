using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Deleting;

/// <inheritdoc />
internal class SchemaDeletingRepository : MongoCollectionRepository<AttributeSchema>, ISchemaDeletingRepository
{
    /// <inheritdoc />
    public SchemaDeletingRepository(DmMongoClient client) : base(client)
    {
    }

    /// <inheritdoc />
    public async Task Delete(Guid schemaId) => await Collection.DeleteOneAsync(Filter.Eq(s => s.Id, schemaId));
}
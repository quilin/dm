using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Deleting;

/// <inheritdoc />
internal class SchemaDeletingService(
    ISchemaReadingService readingService,
    IIntentionManager intentionManager,
    ISchemaDeletingRepository repository) : ISchemaDeletingService
{
    /// <inheritdoc />
    public async Task Delete(Guid schemaId, CancellationToken cancellationToken)
    {
        var schema = await readingService.Get(schemaId, cancellationToken);
        intentionManager.ThrowIfForbidden(AttributeSchemaIntention.Delete, schema);
        await repository.Delete(schemaId, cancellationToken);
    }
}
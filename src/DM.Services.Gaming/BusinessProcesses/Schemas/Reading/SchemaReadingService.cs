using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Reading;

/// <inheritdoc />
internal class SchemaReadingService(
    ISchemaReadingRepository repository,
    IIdentityProvider identityProvider) : ISchemaReadingService
{
    /// <param name="cancellationToken"></param>
    /// <inheritdoc />
    public async Task<IEnumerable<AttributeSchema>> Get(CancellationToken cancellationToken) =>
        await repository.GetSchemata(identityProvider.Current.User.UserId, cancellationToken);

    /// <inheritdoc />
    public async Task<AttributeSchema> Get(Guid schemaId, CancellationToken cancellationToken)
    {
        var attributeSchema = await repository.GetSchema(schemaId, cancellationToken);
        if (attributeSchema == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Schema not found");
        }

        return attributeSchema;
    }
}
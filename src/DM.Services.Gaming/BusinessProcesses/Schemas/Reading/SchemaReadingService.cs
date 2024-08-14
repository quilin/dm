using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Reading;

/// <inheritdoc />
internal class SchemaReadingService : ISchemaReadingService
{
    private readonly ISchemaReadingRepository repository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public SchemaReadingService(
        ISchemaReadingRepository repository,
        IIdentityProvider identityProvider)
    {
        this.repository = repository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AttributeSchema>> Get() =>
        await repository.GetSchemata(identityProvider.Current.User.UserId);

    /// <inheritdoc />
    public async Task<AttributeSchema> Get(Guid schemaId)
    {
        var attributeSchema = await repository.GetSchema(schemaId);
        if (attributeSchema == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Schema not found");
        }

        return attributeSchema;
    }
}
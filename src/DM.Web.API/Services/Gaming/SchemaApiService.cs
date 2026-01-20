using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Schemas.Creating;
using DM.Services.Gaming.BusinessProcesses.Schemas.Deleting;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.BusinessProcesses.Schemas.Updating;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games.Attributes;
using DtoAttributeSchema = DM.Services.Gaming.Dto.Shared.AttributeSchema;

namespace DM.Web.API.Services.Gaming;

/// <inheritdoc />
internal class SchemaApiService(
    ISchemaReadingService readingService,
    ISchemaCreatingService creatingService,
    ISchemaUpdatingService updatingService,
    ISchemaDeletingService deletingService,
    IMapper mapper) : ISchemaApiService
{
    /// <param name="cancellationToken"></param>
    /// <inheritdoc />
    public async Task<ListEnvelope<AttributeSchema>> Get(CancellationToken cancellationToken)
    {
        var schemata = await readingService.Get(cancellationToken);
        return new ListEnvelope<AttributeSchema>(schemata.Select(mapper.Map<AttributeSchema>));
    }

    /// <inheritdoc />
    public async Task<Envelope<AttributeSchema>> Get(Guid schemaId, CancellationToken cancellationToken)
    {
        var schema = await readingService.Get(schemaId, cancellationToken);
        return new Envelope<AttributeSchema>(mapper.Map<AttributeSchema>(schema));
    }

    /// <inheritdoc />
    public async Task<Envelope<AttributeSchema>> Create(
        AttributeSchema schema, CancellationToken cancellationToken)
    {
        var createSchema = mapper.Map<DtoAttributeSchema>(schema);
        var createdSchema = await creatingService.Create(createSchema, cancellationToken);
        return new Envelope<AttributeSchema>(mapper.Map<AttributeSchema>(createdSchema));
    }

    /// <inheritdoc />
    public async Task<Envelope<AttributeSchema>> Update(
        Guid schemaId, AttributeSchema schema, CancellationToken cancellationToken)
    {
        var updateSchema = mapper.Map<DtoAttributeSchema>(schema);
        updateSchema.Id = schemaId;
        var updatedSchema = await updatingService.Update(updateSchema, cancellationToken);
        return new Envelope<AttributeSchema>(mapper.Map<AttributeSchema>(updatedSchema));
    }

    /// <inheritdoc />
    public Task Delete(Guid schemaId, CancellationToken cancellationToken) =>
        deletingService.Delete(schemaId, cancellationToken);
}
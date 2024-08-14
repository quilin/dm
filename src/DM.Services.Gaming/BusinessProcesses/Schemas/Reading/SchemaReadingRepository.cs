using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess;
using DbSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.Gaming.Dto.Shared;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Reading;

/// <inheritdoc cref="ISchemaReadingRepository" />
internal class SchemaReadingRepository :
    MongoCollectionRepository<DbSchema>,
    ISchemaReadingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public SchemaReadingRepository(
        DmDbContext dbContext,
        DmMongoClient client,
        IMapper mapper) : base(client)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AttributeSchema>> GetSchemata(Guid userId)
    {
        var schemata = await Collection
            .Find(Filter.Eq(s => s.Type, SchemaType.Public) | Filter.Eq(s => s.UserId, userId))
            .ToListAsync();

        if (!schemata.Any())
        {
            return Enumerable.Empty<AttributeSchema>();
        }

        var authorIds = schemata
            .Where(s => s.UserId.HasValue)
            .Select(s => s.UserId.Value)
            .ToHashSet();
        var authors = (await GetSchemataAuthors(authorIds)).ToDictionary(u => u.UserId);

        var result = new List<AttributeSchema>(schemata.Count);
        foreach (var schema in schemata)
        {
            var attributeSchema = mapper.Map<AttributeSchema>(schema);
            attributeSchema.Author = schema.UserId.HasValue &&
                                     authors.TryGetValue(schema.UserId.Value, out var author)
                ? author
                : null;
            result.Add(attributeSchema);
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<AttributeSchema> GetSchema(Guid schemaId)
    {
        var schema = await Collection
            .Find(Filter.Eq(s => s.Id, schemaId))
            .FirstOrDefaultAsync();

        if (schema == null)
        {
            return null;
        }

        var result = mapper.Map<AttributeSchema>(schema);
        if (schema.UserId.HasValue)
        {
            result.Author = (await GetSchemataAuthors(new[] {schema.UserId.Value})).First();
        }

        return result;
    }

    /// <inheritdoc />
    private async Task<IEnumerable<GeneralUser>> GetSchemataAuthors(ICollection<Guid> userIds)
    {
        return await dbContext.Users
            .Where(u => userIds.Contains(u.UserId))
            .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }
}
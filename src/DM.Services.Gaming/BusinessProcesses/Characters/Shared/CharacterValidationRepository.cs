using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.DataAccess;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Shared;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using DbSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Shared;

/// <inheritdoc />
internal class CharacterValidationRepository(
    IMapper mapper,
    DmDbContext dbContext,
    DmMongoClient client) : MongoCollectionRepository<DbSchema>(client), ICharacterValidationRepository
{
    /// <inheritdoc />
    public Task<bool> GameRequiresAttributes(CreateCharacter createCharacter, CancellationToken cancellationToken) =>
        dbContext.Games
            .Where(g => g.GameId == createCharacter.GameId)
            .Select(g => g.AttributeSchemaId.HasValue)
            .FirstAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<AttributeSchema> GetGameSchema(Guid gameId, CancellationToken cancellationToken)
    {
        var schemaId = await dbContext.Games
            .Where(g => g.GameId == gameId)
            .Select(g => g.AttributeSchemaId)
            .FirstAsync(cancellationToken);
        var schema = await Collection
            .Find(Filter.Eq(s => s.Id, schemaId.Value))
            .FirstAsync(cancellationToken);
        return mapper.Map<AttributeSchema>(schema);
    }

    /// <inheritdoc />
    public async Task<AttributeSchema> GetCharacterSchema(Guid characterId, CancellationToken cancellationToken)
    {
        var gameId = await dbContext.Characters
            .Where(c => c.CharacterId == characterId)
            .Select(c => c.GameId)
            .FirstAsync(cancellationToken);
        return await GetGameSchema(gameId, cancellationToken);
    }
}
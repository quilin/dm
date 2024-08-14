using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using CharacterAttribute = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.CharacterAttribute;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Updating;

/// <inheritdoc />
internal class CharacterUpdatingRepository : ICharacterUpdatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public CharacterUpdatingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public Task<CharacterToUpdate> Get(Guid characterId)
    {
        return dbContext.Characters
            .Where(c => c.CharacterId == characterId)
            .ProjectTo<CharacterToUpdate>(mapper.ConfigurationProvider)
            .FirstAsync();
    }

    /// <inheritdoc />
    public async Task<Character> Update(
        IUpdateBuilder<DataAccess.BusinessObjects.Games.Characters.Character> updateCharacter,
        IEnumerable<IUpdateBuilder<CharacterAttribute>> attributeChanges)
    {
        var characterId = updateCharacter.AttachTo(dbContext);
        foreach (var attributeChange in attributeChanges)
        {
            attributeChange.AttachTo(dbContext);
        }

        await dbContext.SaveChangesAsync();
        return await dbContext.Characters
            .Where(c => c.CharacterId == characterId)
            .ProjectTo<Character>(mapper.ConfigurationProvider)
            .FirstAsync();
    }

    /// <inheritdoc />
    public async Task<IDictionary<Guid, Guid>> GetAttributeIds(Guid characterId)
    {
        return await dbContext.CharacterAttributes
            .Where(c => c.CharacterId == characterId)
            .ToDictionaryAsync(a => a.AttributeId, a => a.CharacterAttributeId);
    }
}
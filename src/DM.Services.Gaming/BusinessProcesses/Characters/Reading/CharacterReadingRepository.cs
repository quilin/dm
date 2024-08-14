using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Reading;

/// <inheritdoc />
internal class CharacterReadingRepository : ICharacterReadingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public CharacterReadingRepository(
        DmDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Character>> GetCharacters(Guid gameId)
    {
        return await dbContext.Characters
            .Where(c => !c.IsRemoved && c.GameId == gameId)
            .OrderByDescending(c => c.CreateDate)
            .ProjectTo<Character>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public async Task<Character> FindCharacter(Guid characterId)
    {
        return await dbContext.Characters
            .Where(c => !c.IsRemoved && c.CharacterId == characterId)
            .ProjectTo<Character>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }
}
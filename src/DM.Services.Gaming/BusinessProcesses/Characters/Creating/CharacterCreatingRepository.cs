using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using CharacterAttribute = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.CharacterAttribute;
using DbCharacter = DM.Services.DataAccess.BusinessObjects.Games.Characters.Character;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Creating;

/// <inheritdoc />
internal class CharacterCreatingRepository(
    DmDbContext dbContext,
    IMapper mapper) : ICharacterCreatingRepository
{
    /// <inheritdoc />
    public async Task<Character> Create(DbCharacter character, IEnumerable<CharacterAttribute> attributes,
        CancellationToken cancellationToken)
    {
        dbContext.Characters.Add(character);
        dbContext.CharacterAttributes.AddRange(attributes);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await dbContext.Characters
            .Where(c => c.CharacterId == character.CharacterId)
            .ProjectTo<Character>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
    }
}
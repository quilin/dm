using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;
using DbCharacter = DM.Services.DataAccess.BusinessObjects.Games.Characters.Character;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Creating
{
    /// <inheritdoc />
    public class CharacterCreatingRepository : ICharacterCreatingRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public CharacterCreatingRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        
        /// <inheritdoc />
        public async Task<Character> Create(DbCharacter character)
        {
            dbContext.Characters.Add(character);
            await dbContext.SaveChangesAsync();
            return await dbContext.Characters
                .Where(c => c.CharacterId == character.CharacterId)
                .ProjectTo<Character>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}
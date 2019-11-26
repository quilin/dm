using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Gaming.Dto.Internal;
using DM.Services.Gaming.Dto.Output;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.Gaming.BusinessProcesses.Characters.Updating
{
    /// <inheritdoc />
    public class CharacterUpdatingRepository : ICharacterUpdatingRepository
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
        public async Task<Character> Update(IUpdateBuilder<DataAccess.BusinessObjects.Games.Characters.Character> updateCharacter)
        {
            var characterId = updateCharacter.AttachTo(dbContext);
            await dbContext.SaveChangesAsync();
            return await dbContext.Characters
                .Where(c => c.CharacterId == characterId)
                .ProjectTo<Character>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}
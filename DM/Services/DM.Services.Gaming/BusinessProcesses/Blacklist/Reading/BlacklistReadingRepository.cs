using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Gaming.BusinessProcesses.Blacklist.Reading
{
    /// <inheritdoc />
    internal class BlacklistReadingRepository : IBlacklistReadingRepository
    {
        /// <inheritdoc />
        public Task<IEnumerable<GeneralUser>> Get(Guid gameId)
        {
            throw new NotImplementedException();
        }
    }
}
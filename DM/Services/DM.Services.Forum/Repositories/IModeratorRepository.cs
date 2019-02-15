using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Forum.Repositories
{
    public interface IModeratorRepository
    {
        Task<IEnumerable<GeneralUser>> Get(Guid forumId);
    }
}
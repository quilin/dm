using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Web.Classic.Views.GameCommentaries.Commentary;

namespace DM.Web.Classic.Views.GameCommentaries
{
    public interface IGameCommentariesViewModelBuilder
    {
        Task<GameCommentariesViewModel> Build(Guid gameId, int entityNumber);
        Task<IEnumerable<GameCommentaryViewModel>> BuildList(Guid gameId, int entityNumber);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.Dto;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.GameCommentaries.Commentary
{
    public interface IGameCommentaryViewModelBuilder
    {
        Task<GameCommentaryViewModel> Build(Comment comment, Game game,
            IDictionary<Guid, IEnumerable<string>> characterNames);
    }
}
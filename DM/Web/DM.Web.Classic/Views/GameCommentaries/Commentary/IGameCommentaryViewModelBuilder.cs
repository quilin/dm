using System;
using System.Collections.Generic;
using DM.Services.Common.Dto;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.GameCommentaries.Commentary
{
    public interface IGameCommentaryViewModelBuilder
    {
        GameCommentaryViewModel Build(Comment comment, Game game,
            IDictionary<Guid, IEnumerable<string>> characterNames);
    }
}
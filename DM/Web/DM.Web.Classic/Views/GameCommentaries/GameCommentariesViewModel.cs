using System;
using System.Collections.Generic;
using DM.Web.Classic.Views.GameCommentaries.Commentary;
using DM.Web.Classic.Views.GameCommentaries.CreateCommentary;

namespace DM.Web.Classic.Views.GameCommentaries
{
    public class GameCommentariesViewModel
    {
        public Guid GameId { get; set; }
        public string GameTitle { get; set; }

        public int TotalPagesCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int EntityNumber { get; set; }

        public IEnumerable<GameCommentaryViewModel> ModuleCommentaries { get; set; }
        public GameCommentaryCreateForm CreateForm { get; set; }
    }
}
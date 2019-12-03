using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;
using DM.Web.Classic.Views.GamesList.GamesListItem;

namespace DM.Web.Classic.Views.GamesList
{
    public class GamesListViewModel
    {
        public GameStatus? Status { get; set; }
        public Guid? TagId { get; set; }
        public string TagTitle { get; set; }

        public string PageTitle { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPagesCount { get; set; }
        public int PageSize { get; set; }
        public int EntityNumber { get; set; }

        public IEnumerable<GamesListItemViewModel> Games { get; set; }
    }
}
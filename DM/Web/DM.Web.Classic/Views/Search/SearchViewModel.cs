using System.Collections.Generic;
using DM.Services.Core.Dto;

namespace DM.Web.Classic.Views.Search
{
    public class SearchViewModel
    {
        public string Query { get; set; }
        public SearchLocation Location { get; set; }
        public IEnumerable<SearchEntryViewModel> Results { get; set; }
        public PagingResult Paging { get; set; }
    }
}
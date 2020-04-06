using System.Collections.Generic;

namespace DM.Web.Classic.Views.Search
{
    public class SearchViewModel
    {
        public SearchForm Form { get; set; }
        public IEnumerable<SearchEntryViewModel> Results { get; set; }
    }
}
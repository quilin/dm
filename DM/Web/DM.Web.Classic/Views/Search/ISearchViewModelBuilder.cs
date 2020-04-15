using System.Collections.Generic;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Search
{
    public interface ISearchViewModelBuilder
    {
        Task<SearchViewModel> Build(string query, SearchLocation location, int entityNumber);
        Task<IDictionary<string, string>> BuildAutocomplete(string query);
    }
}
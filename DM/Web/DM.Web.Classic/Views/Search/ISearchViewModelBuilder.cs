using System.Threading.Tasks;

namespace DM.Web.Classic.Views.Search
{
    public interface ISearchViewModelBuilder
    {
        Task<SearchViewModel> Build();
        Task<SearchViewModel> Build(SearchForm searchForm, int entityNumber);
    }
}
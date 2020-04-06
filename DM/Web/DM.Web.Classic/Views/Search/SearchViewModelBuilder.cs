using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Search.BusinessProcesses;

namespace DM.Web.Classic.Views.Search
{
    public class SearchViewModelBuilder : ISearchViewModelBuilder
    {
        private readonly ISearchService searchService;
        private readonly ISearchEntryViewModelBuilder searchEntryViewModelBuilder;

        public SearchViewModelBuilder(
            ISearchService searchService,
            ISearchEntryViewModelBuilder searchEntryViewModelBuilder)
        {
            this.searchService = searchService;
            this.searchEntryViewModelBuilder = searchEntryViewModelBuilder;
        }

        public Task<SearchViewModel> Build() => Task.FromResult(new SearchViewModel
        {
            Form = new SearchForm(),
            Results = null
        });

        public async Task<SearchViewModel> Build(SearchForm searchForm, int entityNumber)
        {
            var (results, _) = await searchService.Search(searchForm.Query,
                searchForm.SearchEntityType == SearchEntityType.Unknown
                    ? (SearchEntityType?) null
                    : searchForm.SearchEntityType, new PagingQuery {Number = entityNumber});
            
            return new SearchViewModel
            {
                Form = searchForm,
                Results = results.Select(searchEntryViewModelBuilder.Build)
            };
        }
    }
}
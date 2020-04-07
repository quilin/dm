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
            var entityTypes = searchForm.Location switch
            {
                SearchLocation.Everywhere => Enumerable.Empty<SearchEntityType>(),
                SearchLocation.Forum => new[] {SearchEntityType.Topic, SearchEntityType.ForumComment},
                SearchLocation.Games => new[] {SearchEntityType.Game},
                SearchLocation.Community => new[] {SearchEntityType.User}
            };

            var (results, _) = await searchService.Search(searchForm.Query, entityTypes,
                new PagingQuery {Number = entityNumber});
            
            return new SearchViewModel
            {
                Form = searchForm,
                Results = results.Select(searchEntryViewModelBuilder.Build)
            };
        }
    }
}
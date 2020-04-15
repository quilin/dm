using System.Collections.Generic;
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

        public async Task<SearchViewModel> Build(string query, SearchLocation location, int entityNumber)
        {
            var entityTypes = location switch
            {
                SearchLocation.Everywhere => Enumerable.Empty<SearchEntityType>(),
                SearchLocation.Forum => new[] {SearchEntityType.Topic, SearchEntityType.ForumComment},
                SearchLocation.Games => new[] {SearchEntityType.Game},
                SearchLocation.Community => new[] {SearchEntityType.User}
            };

            var (results, paging) = await searchService.Search(query, entityTypes,
                new PagingQuery {Number = entityNumber});

            return new SearchViewModel
            {
                Query = query,
                Location = location,
                Results = results.Select(searchEntryViewModelBuilder.Build),
                Paging = paging
            };
        }

        public async Task<IDictionary<string, string>> BuildAutocomplete(string query)
        {
            var (results, _) = await searchService.Search(query, new[] {SearchEntityType.User}, new PagingQuery
            {
                Skip = 0,
                Size = 5
            });
            return results.ToDictionary(r => r.OriginalTitle, r => r.OriginalTitle);
        }
    }
}
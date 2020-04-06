using DM.Services.Search.Dto;

namespace DM.Web.Classic.Views.Search
{
    public class SearchEntryViewModelBuilder : ISearchEntryViewModelBuilder
    {
        public SearchEntryViewModel Build(FoundEntity foundEntity)
        {
            return new SearchEntryViewModel
            {
                Id = foundEntity.Id,
                // ?? Author = userViewModelBuilder.Build(foundEntity.Author),
                Title = foundEntity.FoundTitle,
                Text = foundEntity.FoundText
            };
        }
    }
}
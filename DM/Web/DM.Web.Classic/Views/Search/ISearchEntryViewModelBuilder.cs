using DM.Services.Search.Dto;

namespace DM.Web.Classic.Views.Search
{
    public interface ISearchEntryViewModelBuilder
    {
        SearchEntryViewModel Build(FoundEntity foundEntity);
    }
}
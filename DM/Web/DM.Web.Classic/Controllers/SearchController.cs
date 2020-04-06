using System.Threading.Tasks;
using DM.Web.Classic.Views.Search;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchViewModelBuilder searchViewModelBuilder;

        public SearchController(
            ISearchViewModelBuilder searchViewModelBuilder)
        {
            this.searchViewModelBuilder = searchViewModelBuilder;
        }
        
        public async Task<IActionResult> Index() => View(await searchViewModelBuilder.Build());

        [HttpPost]
        public async Task<IActionResult> Index(SearchForm searchForm, int entityNumber)
        {
            var searchViewModel = await searchViewModelBuilder.Build(searchForm, entityNumber);
            return View(searchViewModel);
        }
    }
}
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Core.Exceptions;
using DM.Web.Classic.Extensions.RequestExtensions;
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

        [HttpGet]
        public async Task<IActionResult> Index(string query, SearchLocation location, int entityNumber)
        {
            var searchViewModel = await searchViewModelBuilder.Build(query, location, entityNumber);
            return Request.IsAjaxRequest()
                ? (IActionResult) PartialView("SearchEntries", searchViewModel)
                : View(searchViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UserAutocomplete(string query)
        {
            var result = await searchViewModelBuilder.BuildAutocomplete(query);
            return result.Any()
                ? Ok(result)
                : throw new HttpException(HttpStatusCode.NotFound, "Nothing found");
        }
    }
}
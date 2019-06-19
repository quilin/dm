using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.Classic.Controllers
{
    public class SearchController : DmControllerBase
    {
        private readonly IUserSearchService userSearchService;

        public SearchController(
            IUserSearchService userSearchService
            )
        {
            this.userSearchService = userSearchService;
        }

        [HttpPost]
        public IActionResult AutocompleteUser(string value)
        {
            var result = userSearchService.Select(value);
            return result.Count == 0
                       ? AjaxError(HttpStatusCode.NotFound, null)
                       : Json(result);
        }

        [HttpPost]
        public IActionResult SearchUser(string value)
        {
            var result = userSearchService.Search(value);
            return result.Count == 0
                       ? AjaxError(HttpStatusCode.NotFound, null)
                       : Json(result);
        }

        [HttpGet]
        public ActionResult Tags(Guid tagId)
        {
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return new EmptyResult();
        }
    }
}
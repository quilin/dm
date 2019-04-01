using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.SearchEngine.Repositories;
using DM.Web.API.Dto.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Common
{
    /// <summary>
    /// Search
    /// </summary>
    [Route("v1/search")]
    public class SearchController : Controller
    {
        private readonly ISearchEngineRepository repository;

        /// <inheritdoc />
        public SearchController(
            ISearchEngineRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListEnvelope<object>> Search([FromQuery] string query)
        {
            // TODO: Move to service and massage it
            var (entities, _) = await repository.Search(query, 0, 10, new[] {UserRole.Guest}, Guid.Empty);
            return new ListEnvelope<object>(entities);
        }
    }
}
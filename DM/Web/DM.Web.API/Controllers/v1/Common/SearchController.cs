using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Search.BusinessProcesses;
using DM.Web.API.Dto.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Common
{
    /// <inheritdoc />
    [ApiController]
    [Route("v1/search")]
    [ApiExplorerSettings(GroupName = "Common")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService service;

        /// <inheritdoc />
        public SearchController(
            ISearchService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListEnvelope<object>> Search([FromQuery] string query, [FromQuery] PagingQuery q)
        {
            // TODO: Move to service and massage it
            var (entities, paging) = await service.Search(query, Enumerable.Empty<SearchEntityType>(), q);
            return new ListEnvelope<object>(entities, new Paging(paging));
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Search.Repositories;
using DM.Web.API.Dto.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Common
{
    /// <summary>
    /// Search
    /// </summary>
    [Route("v1/search")]
    [ApiExplorerSettings(GroupName = "Common")]
    public class SearchController : Controller
    {
        private readonly ISearchEngineRepository repository;
        private readonly IIdentityProvider identityProvider;

        /// <inheritdoc />
        public SearchController(
            ISearchEngineRepository repository,
            IIdentityProvider identityProvider)
        {
            this.repository = repository;
            this.identityProvider = identityProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListEnvelope<object>> Search([FromQuery] string query)
        {
            // TODO: Move to service and massage it
            var currentUserRole = identityProvider.Current.User.Role;
            var userRoles = currentUserRole == UserRole.Guest
                ? Enumerable.Repeat(UserRole.Guest, 1)
                : Enum.GetValues(typeof(UserRole))
                    .Cast<UserRole>()
                    .Where(r => currentUserRole.HasFlag(r));
            var (entities, total) = await repository.Search(query, 0, 10, userRoles, Guid.Empty);
            return new ListEnvelope<object>(entities, new Paging(PagingResult.Create(total, 1, 10)));
        }
    }
}
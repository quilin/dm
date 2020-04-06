using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Search.Dto;
using DM.Services.Search.Repositories;

namespace DM.Services.Search.BusinessProcesses
{
    /// <inheritdoc />
    public class SearchService : ISearchService
    {
        private readonly ISearchEngineRepository searchEngineRepository;
        private readonly IIdentityProvider identityProvider;

        /// <inheritdoc />
        public SearchService(
            ISearchEngineRepository searchEngineRepository,
            IIdentityProvider identityProvider)
        {
            this.searchEngineRepository = searchEngineRepository;
            this.identityProvider = identityProvider;
        }
        
        /// <inheritdoc />
        public async Task<(IEnumerable<FoundEntity> results, PagingResult paging)> Search(string query,
            SearchEntityType? type, PagingQuery pagingQuery)
        {
            var identity = identityProvider.Current;
            var pagingData = new PagingData(pagingQuery, identity.Settings.Paging.EntitiesPerPage, int.MaxValue);
            var userRoles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>()
                .Where(r => identity.User.Role.HasFlag(r));
            var (entities, totalCount) = await searchEngineRepository.Search(
                query, type, pagingData, userRoles, identity.User.UserId);
            
            pagingData = new PagingData(pagingQuery, identity.Settings.Paging.EntitiesPerPage, totalCount);
            return (entities, pagingData.Result);
        }
    }
}
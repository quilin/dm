using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Search.Dto;
using DM.Services.Search.Repositories;

namespace DM.Services.Search.BusinessProcesses;

/// <inheritdoc />
internal class SearchService(
    ISearchEngineRepository searchEngineRepository,
    IIdentityProvider identityProvider)
    : ISearchService
{
    /// <inheritdoc />
    public async Task<(IEnumerable<FoundEntity> results, PagingResult paging)> Search(string query,
        IEnumerable<SearchEntityType> types, PagingQuery pagingQuery)
    {
        var identity = identityProvider.Current;
        var pageSize = identity.Settings.Paging.EntitiesPerPage;
        if (string.IsNullOrWhiteSpace(query))
        {
            return (Enumerable.Empty<FoundEntity>(), PagingResult.Empty(pageSize));
        }

        var pagingData = new PagingData(pagingQuery, pageSize, int.MaxValue);
        var userRoles = Enum.GetValues<UserRole>().Where(r => identity.User.Role.HasFlag(r));
        var (entities, totalCount) = await searchEngineRepository.Search(
            query, types, pagingData, userRoles, identity.User.UserId);

        pagingData = new PagingData(pagingQuery, pageSize, totalCount);
        return (entities, pagingData.Result);
    }
}
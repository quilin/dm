using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.Search.Dto;
using Microsoft.Extensions.Logging;
using OpenSearch.Client;

namespace DM.Services.Search.Repositories;

/// <inheritdoc />
internal class SearchEngineRepository : ISearchEngineRepository
{
    private readonly IOpenSearchClient client;
    private readonly ILogger<SearchEngineRepository> logger;

    /// <inheritdoc />
    public SearchEngineRepository(
        IOpenSearchClient client,
        ILogger<SearchEngineRepository> logger)
    {
        this.client = client;
        this.logger = logger;
    }

    private static readonly Fuzziness SearchFuzziness = Fuzziness.EditDistance(1);

    /// <inheritdoc />
    public async Task<(IEnumerable<FoundEntity> entities, int totalCount)> Search(string query,
        IEnumerable<SearchEntityType> types, PagingData pagingData, IEnumerable<UserRole> roles, Guid userId)
    {
        var searchResponse = await client.SearchAsync<SearchEntity>(s => s
            .Source(sf => sf.Excludes(e => e.Fields(
                f => f.AuthorizedRoles, f => f.AuthorizedUsers, f => f.UnauthorizedUsers)))
            .Query(q =>
            {
                var querySearch =
                    q.Match(mt => mt.Field(f => f.Text)
                        .Query(query)
                        .Fuzziness(SearchFuzziness)
                        .Boost(1)) ||
                    q.Match(mt => mt.Field(f => f.Title)
                        .Query(query)
                        .Fuzziness(SearchFuzziness)
                        .Boost(3)) ||
                    q.Prefix(pr => pr.Field(f => f.Title)
                        .Value(query)
                        .Boost(2));

                var searchEntityTypes = types as SearchEntityType[] ?? types.ToArray();
                if (searchEntityTypes.Any())
                {
                    querySearch = querySearch && q.Terms(t => t.Field(f => f.EntityType).Terms(searchEntityTypes));
                }

                var authorizeSearch =
                    q.Terms(t => t.Field(f => f.AuthorizedRoles).Terms(roles.Cast<int>()).Boost(0)) ||
                    q.Terms(t => t.Field(f => f.AuthorizedUsers).Terms(userId).Boost(0)) ||
                    !q.Terms(t => t.Field(f => f.UnauthorizedUsers).Terms(userId).Boost(0));

                return querySearch && authorizeSearch;
            })
            .Sort(so => so.Descending(SortSpecialField.Score))
            .Highlight(h => h
                .Fields(
                    f => f
                        .Field(ff => ff.Title)
                        .PreTags("<mark>")
                        .PostTags("</mark>"),
                    f => f
                        .Field(ff => ff.Text)
                        .PreTags("<mark>")
                        .PostTags("</mark>")))
            .From(pagingData.Skip)
            .Size(pagingData.Take));

        if (searchResponse is not { IsValid: true })
        {
            logger.LogError(searchResponse.OriginalException,
                "The search for query {SearchQuery} has resulted in error", query);
            throw new HttpException(HttpStatusCode.InternalServerError, "Search engine error!");
        }

        return (searchResponse.Hits
            .Select(h => new FoundEntity
            {
                Id = h.Source.Id,
                Type = h.Source.EntityType,
                FoundTitle = h.Highlight.TryGetValue(nameof(SearchEntity.Title).ToLower(), out var titleHits)
                    ? titleHits.First()
                    : h.Source.Title,
                OriginalTitle = h.Source.Title,
                FoundText = h.Highlight.TryGetValue(nameof(SearchEntity.Text).ToLower(), out var textHits)
                    ? string.Join("<br />", textHits.Where(hl => !string.IsNullOrWhiteSpace(hl)))
                    : h.Source.Text
            }), (int) searchResponse.Total);
    }
}
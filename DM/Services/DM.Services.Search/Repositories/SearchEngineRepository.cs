using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.Search.Dto;
using Nest;

namespace DM.Services.Search.Repositories
{
    /// <inheritdoc />
    public class SearchEngineRepository : ISearchEngineRepository
    {
        private readonly IElasticClient client;

        /// <inheritdoc />
        public SearchEngineRepository(
            IElasticClient client)
        {
            this.client = client;
        }

        private static readonly Fuzziness SearchFuzziness = Fuzziness.EditDistance(1);

        /// <inheritdoc />
        public async Task<(IEnumerable<FoundEntity> entities, int totalCount)> Search(string query, int skip, int take,
            IEnumerable<UserRole> roles, Guid userId)
        {
            var searchResponse = await client.SearchAsync<SearchEntity>(s => s
                .Source(sf => sf.Excludes(e => e.Fields(f => f.AuthorizedRoles, f => f.AuthorizedUsers)))
                .Query(q =>
                    (q.Match(mt => mt.Field(f => f.Text)
                         .Query(query)
                         .Fuzziness(SearchFuzziness)
                         .Boost(5)) ||
                     q.Match(mt => mt.Field(f => f.Title)
                         .Query(query)
                         .Fuzziness(SearchFuzziness))) &&
                    (q.Terms(t => t.Field(f => f.AuthorizedRoles).Terms(roles.Cast<int>()).Boost(0)) ||
                     q.Terms(t => t.Field(f => f.AuthorizedUsers).Terms(userId).Boost(0))))
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
                .From(skip)
                .Size(take));

            return (searchResponse.Hits
                .Select(h => new FoundEntity
                {
                    Id = h.Source.Id,
                    Type = h.Source.EntityType,
                    FoundTitle = h.Highlights.TryGetValue(nameof(SearchEntity.Title).ToLower(), out var titleHit)
                        ? titleHit.Highlights.First()
                        : h.Source.Title,
                    FoundText = h.Highlights.TryGetValue(nameof(SearchEntity.Text).ToLower(), out var textHit)
                        ? textHit.Highlights.First()
                        : h.Source.Text
                }), (int) searchResponse.Total);
        }
    }
}
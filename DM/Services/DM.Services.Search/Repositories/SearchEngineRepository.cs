using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.Search.Configuration;
using DM.Services.Search.Dto;
using Microsoft.Extensions.Options;
using Nest;

namespace DM.Services.Search.Repositories
{
    /// <inheritdoc />
    public class SearchEngineRepository : ISearchEngineRepository
    {
        private readonly IElasticClient client;
        private readonly SearchEngineConfiguration configuration;

        /// <inheritdoc />
        public SearchEngineRepository(
            IElasticClient client,
            IOptions<SearchEngineConfiguration> options)
        {
            this.client = client;
            configuration = options.Value;
        }

        /// <inheritdoc />
        public async Task Index(params SearchEntity[] entities)
        {
            await DeclareIndex();
            await client.IndexManyAsync(entities);
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<FoundEntity> entities, int totalCount)> Search(string query, int skip, int take,
            IEnumerable<UserRole> roles, Guid userId)
        {
            var searchResponse = await client.SearchAsync<SearchEntity>(s => s
                .Source(sf => sf.Excludes(e => e.Fields(f => f.AuthorizedRoles, f => f.AuthorizedUsers)))
                .Query(q =>
                    q.Match(mt => mt.Field(f => f.Text)
                        .Query(query)
                        .Fuzziness(Fuzziness.EditDistance(2))) &&
                    (q.Terms(t => t.Field(f => f.AuthorizedRoles).Terms(roles).Boost(0)) ||
                        q.Terms(t => t.Field(f => f.AuthorizedUsers).Terms(userId).Boost(0))))
                .Sort(so => so.Descending(SortSpecialField.Score))
                .Highlight(h => h
                    .Fields(f => f
                        .Field(ff => ff.Text)
                        .PreTags("<mark>")
                        .PostTags("</mark>")))
                .From(skip)
                .Size(take));

            return (searchResponse.Hits.Zip(searchResponse.Documents,
                    (hit, doc) => (Hit: hit, Document: doc))
                .Select(p => new FoundEntity
                {
                    Id = p.Document.Id,
                    Type = p.Document.EntityType,
                    FoundText = p.Hit.Highlights.First().Value.Highlights.First()
                }), (int) searchResponse.Total);
        }

        private async Task DeclareIndex()
        {
            if ((await client.IndexExistsAsync(configuration.IndexName)).Exists)
            {
                return;
            }

            await client.CreateIndexAsync(configuration.IndexName, i => i
                .Settings(s => s
                    .Analysis(a => a
                        .Analyzers(an => an
                            .Custom("dm_analyzer", ca => ca
                                .CharFilters("html_strip")
                                .Tokenizer("standard")
                                .Filters("standard", "lowercase", "stop"))
                            .Custom("dm_search_analyzer", ca => ca
                                .Tokenizer("standard")
                                .Filters("standard", "lowercase", "stop")))))
                .Mappings(m => m.Map<SearchEntity>(mm => mm
                    .AutoMap()
                    .Properties(p => p
                        .Text(t => t
                            .Name(n => n.Text)
                            .Analyzer("dm_analyzer")
                            .SearchAnalyzer("dm_search_analyzer"))))));
        }
    }
}
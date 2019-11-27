using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.Search.Configuration;
using Nest;

namespace DM.Services.Search.Consumer.Implementation
{
    /// <inheritdoc />
    public class IndexingRepository : IIndexingRepository
    {
        private readonly IElasticClient client;

        /// <inheritdoc />
        public IndexingRepository(
            IElasticClient client)
        {
            this.client = client;
        }

        /// <inheritdoc />
        public async Task Index(params SearchEntity[] entities)
        {
            await DeclareIndex();
            await client.IndexManyAsync(entities);
        }

        /// <inheritdoc />
        public Task Delete(Guid entityId)
        {
            return client.DeleteAsync<SearchEntity>(entityId);
        }

        /// <inheritdoc />
        public Task DeleteByParent(Guid parentEntityId)
        {
            return client.DeleteByQueryAsync<SearchEntity>(d => d.Query(q => q
                .Term(t => t
                    .Field(f => f.ParentEntityId)
                    .Value(parentEntityId))));
        }

        /// <inheritdoc />
        public Task UpdateByParent(Guid parentEntityId, IEnumerable<UserRole> roles)
        {
            return client.UpdateByQueryAsync<SearchEntity>(d => d.Query(q => q
                .Term(t => t
                    .Field(f => f.ParentEntityId)
                    .Value(parentEntityId)))
                .Script($"ctx._source.authorizedRoles = [{string.Join(",", roles.Cast<int>())}]"));
        }

        /// <inheritdoc />
        public Task UpdateByParent(Guid parentEntityId, IEnumerable<Guid> userIds)
        {
            return client.UpdateByQueryAsync<SearchEntity>(d => d.Query(q => q
                    .Term(t => t
                        .Field(f => f.ParentEntityId)
                        .Value(parentEntityId)))
                .Script($"ctx._source.authorizedUsers = [{string.Join(",", userIds)}]"));
        }

        private async Task DeclareIndex()
        {
            if ((await client.IndexExistsAsync(SearchEngineConfiguration.IndexName)).Exists)
            {
                return;
            }

            await client.CreateIndexAsync(SearchEngineConfiguration.IndexName, i => i
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
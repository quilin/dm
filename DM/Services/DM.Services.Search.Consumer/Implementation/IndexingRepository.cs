using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.Search.Configuration;
using Microsoft.Extensions.Logging;
using Nest;

namespace DM.Services.Search.Consumer.Implementation
{
    /// <inheritdoc />
    internal class IndexingRepository : IIndexingRepository
    {
        private readonly IElasticClient client;
        private readonly ILogger<IndexingRepository> logger;

        /// <inheritdoc />
        public IndexingRepository(
            IElasticClient client,
            ILogger<IndexingRepository> logger)
        {
            this.client = client;
            this.logger = logger;
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
            var indexExistsResponse = await client.Indices.ExistsAsync(SearchEngineConfiguration.IndexName);
            if (indexExistsResponse is { IsValid: true, Exists: true })
            {
                return;
            }

            logger.LogInformation("Search index was not created, creating it!");
            var createIndexResponse = await client.Indices.CreateAsync(SearchEngineConfiguration.IndexName, i => i
                .Settings(s => s
                    .Analysis(a => a
                        .Analyzers(an => an
                            .Custom("dm_analyzer", ca => ca
                                .CharFilters("html_strip")
                                .Tokenizer("standard")
                                .Filters("lowercase", "stop"))
                            .Custom("dm_search_analyzer", ca => ca
                                .Tokenizer("standard")
                                .Filters("lowercase", "stop")))))
                .Map<SearchEntity>(m => m
                    .AutoMap()
                    .Properties(p => p
                        .Text(t => t
                            .Name(n => n.Text)
                            .Analyzer("dm_analyzer")
                            .SearchAnalyzer("dm_search_analyzer")))));
            if (createIndexResponse is not { IsValid: true, Index: SearchEngineConfiguration.IndexName })
            {
                logger.LogError(createIndexResponse.OriginalException, 
                    "Unable to create search index for reason: {Reason}",
                    createIndexResponse.ServerError.Error.Reason);
            }
        }
    }
}